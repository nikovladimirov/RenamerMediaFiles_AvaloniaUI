using System;
using System.Collections.Generic;

namespace RenamerMediaFiles.Models
{
    public class MetadataItemModel : ModelBase
    {
        private readonly List<string> _metaInfoCaptions;
        private bool _selected;

        public DateTime SourceDateTime { get; }
        public IReadOnlyCollection<string> MetaInfoCaptions => _metaInfoCaptions;
        public string NewFileName { get; set; }

        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public string MetaInfoCaptionsDisplay => string.Join("; ", _metaInfoCaptions);

        public MetadataItemModel(string metaInfoCaption, DateTime sourceDateTime)
        {
            _metaInfoCaptions = new List<string> { metaInfoCaption };
            SourceDateTime = sourceDateTime;
        }

        public void AddMetaInfoCaption(string metaInfoCaption)
        {
            if (!_metaInfoCaptions.Contains(metaInfoCaption))
                _metaInfoCaptions.Add(metaInfoCaption);
        }
    }
}