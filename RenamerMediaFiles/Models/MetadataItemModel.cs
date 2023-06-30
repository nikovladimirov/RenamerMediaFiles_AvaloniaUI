using System;
using System.Collections.Generic;
using RenamerMediaFiles.Helpers;

namespace RenamerMediaFiles.Models
{
    public class MetadataItemModel : ModelBase 
    {
        private List<string> _dateSources;
        private bool _selected;

        public DateTime SourceDateTime { get; }
        public IReadOnlyCollection<string> DateSources => _dateSources;
        public string NewFileName { get; set; }
        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }
        public string DateSourceDisplayValue => string.Join("; ", _dateSources);

        public MetadataItemModel(DateTime sourceDateTime, string dateSource, bool replaceFullName, string newNameFormat, string oldAdditionalName)
        {
            SourceDateTime = sourceDateTime;
            _dateSources = new List<string> { dateSource };
        
            if (replaceFullName)
            {
                NewFileName = SourceDateTime.ToString(newNameFormat);
                return;
            }

            NewFileName = $"{SourceDateTime.ToString(newNameFormat)}{(string.IsNullOrEmpty(oldAdditionalName) ? "" : " ")}{oldAdditionalName}";
        }
        
        public void AddDateSource(string dateSource)
        {
            if(!_dateSources.Contains(dateSource))
                _dateSources.Add(dateSource);
        }
    }
}