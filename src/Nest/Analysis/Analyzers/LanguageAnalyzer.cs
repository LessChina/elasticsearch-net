﻿using System.Collections.Generic;
using Elasticsearch.Net;
using Newtonsoft.Json;

namespace Nest
{
	/// <summary>
	/// A set of analyzers aimed at analyzing specific language text.
	/// </summary>
	public interface ILanguageAnalyzer : IAnalyzer
	{
		/// <summary>
		/// A list of stopword to initialize the stop filter with. Defaults to the english stop words.
		/// </summary>
		[JsonProperty("stopwords")]
		[JsonConverter(typeof(StopWordsJsonConverter))]
		StopWords StopWords { get; set; }

		/// <summary>
		/// The stem_exclusion parameter allows you to specify an array of lowercase words that should not be stemmed.
		/// </summary>
		[JsonProperty("stem_exclusion")]
		IEnumerable<string> StemExclusionList { get; set; }

		/// <summary>
		/// A path (either relative to config location, or absolute) to a stopwords file configuration.
		/// </summary>
		[JsonProperty("stopwords_path")]
		string StopwordsPath { get; set; }
	}

	/// <inheritdoc/>
	public class LanguageAnalyzer : AnalyzerBase, ILanguageAnalyzer
	{
		private string _type = "language";
		public override string Type
		{
			get => _type;
			protected set { this._type = value; this.Language = value.ToEnum<Language>(); }
		}

		/// <inheritdoc/>
		public StopWords StopWords { get; set; }

		/// <inheritdoc/>
		public IEnumerable<string> StemExclusionList { get; set; }

		/// <inheritdoc/>
		[JsonIgnore]
		public Language? Language {
			get => _type.ToEnum<Language>();
			set => _type = value.GetStringValue().ToLowerInvariant();
		}

		/// <inheritdoc/>
		public string StopwordsPath { get; set; }
	}

	/// <inheritdoc/>
	public class LanguageAnalyzerDescriptor :
		AnalyzerDescriptorBase<LanguageAnalyzerDescriptor, ILanguageAnalyzer>, ILanguageAnalyzer
	{
		private string _type = "language";
		protected override string Type => _type;

		StopWords ILanguageAnalyzer.StopWords { get; set; }
		IEnumerable<string> ILanguageAnalyzer.StemExclusionList { get; set; }
		string ILanguageAnalyzer.StopwordsPath { get; set; }

		public LanguageAnalyzerDescriptor Language(Language? language) => Assign(a => this._type = language?.GetStringValue().ToLowerInvariant());

		public LanguageAnalyzerDescriptor StopWords(StopWords stopWords) => Assign(a => a.StopWords = stopWords);

		public LanguageAnalyzerDescriptor StopWords(params string[] stopWords) => Assign(a => a.StopWords = stopWords);

		public LanguageAnalyzerDescriptor StopWords(IEnumerable<string> stopWords) => Assign(a => a.StopWords = stopWords.ToListOrNullIfEmpty());

	}
}
