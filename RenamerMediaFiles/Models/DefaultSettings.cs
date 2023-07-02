using System.Collections.Generic;
using RenamerMediaFiles.Helpers;

namespace RenamerMediaFiles.Models
{
    public static class DefaultSettings
    {
        public const string ConfigPath = "settings.json";
        
        public const string RootPath = "Select folder";
        public const string NewNameFormat ="yyyy-MM-dd HHmmss";
        public const string ExtensionText = "jpg;jpeg;mp4;heic;mov";
        
        public static readonly string[] RemoveByMask = new string[]
        {
            "^IMG[\\(\\)\\d _-]+(edit|COVER|BURST)?[\\(\\)\\d _-]+(COVER)?",
            "^(VID|SVID|SL_MO_VID|HwVideoEditor)[\\(\\)\\d _-]+(save|HSR)?[\\d_]*",
            "^WP[\\.\\(\\)\\d _-]+_Rich",            
            "^(p|P|PC|X|G|GH|GOPR|IMAG|S|SNC|DSCN|DSC|DSCF|DSC_|WP|YDXJ|YIAC|YI|doc)[\\d_-]+( \\(Large\\))?",
            "^WhatsApp Video [\\(\\)\\d _-]+ at [\\.\\(\\)\\d _-]+",
            "^WhatsApp Image [\\(\\)\\d _-]+ at [\\.\\(\\)\\d _-]+",
            "^video_\\d+@?[\\.\\(\\)\\d _-]+",
            "^[\\.\\d _-]+(\\(Large\\))",
            "^[\\.\\(\\)\\d _-]+(LLS|BURST\\d+|IMG_\\d+|Richtone\\(HDR\\))",
            "^\\d[\\.\\(\\)\\d _A-Fa-f-]*(IMG_\\d+)?"
        };

        public static readonly Dictionary<string, MetadataInfoModel> DefaultMetadataInfos =
            new Dictionary<string, MetadataInfoModel>
            {
                {
                    DateSource.Exif_IFD0,
                    new MetadataInfoModel(DateSource.Exif_IFD0,
                        "Exif IFD0",
                        "Date/Time",
                        "yyyy:MM:dd HH:mm:ss",
                        0)
                },
                {
                    DateSource.Exif_SubIFD,
                    new MetadataInfoModel(DateSource.Exif_SubIFD,
                        "Exif SubIFD",
                        "Date/Time Original",
                        "yyyy:MM:dd HH:mm:ss",
                        0)
                },
                {
                    DateSource.QuickTime_Metadata,
                    new MetadataInfoModel(DateSource.QuickTime_Metadata,
                        "QuickTime Metadata Header",
                        "Creation Date",
                        "ddd MMM dd HH:mm:ss K yyyy",
                        3)
                },
                {
                    DateSource.QuickTime_Movie,
                    new MetadataInfoModel(DateSource.QuickTime_Movie,
                        "QuickTime Movie Header",
                        "Created",
                        "ddd MMM dd HH:mm:ss yyyy",
                        3)
                },
                {
                    DateSource.QuickTime_Track,
                    new MetadataInfoModel(DateSource.QuickTime_Track,
                        "QuickTime Track Header",
                        "Created",
                        "ddd MMM dd HH:mm:ss yyyy",
                        3)
                },
            };

    }
}