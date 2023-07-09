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
        public const bool ChangeNameByMasks = true;
        
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
            "^\\d[\\.\\(\\)\\d _-]*(IMG_\\d+)?",
            "^[\\dA-Fa-f][\\.\\(\\)\\d_A-Fa-f-][\\.\\(\\)\\d_-]*(IMG_\\d+)?"
        };

        public static readonly List<MetaDateTimeExtension> MetaDateTimeExtensions =
            new List<MetaDateTimeExtension>
            {
                new MetaDateTimeExtension(MetadataCaptions.GoProPhoto, MetaTypes.AttributeValue, "Exif IFD0", "Make", "GoPro", 3),
                new MetaDateTimeExtension(MetadataCaptions.QuickTime_Metadata, MetaTypes.AttributeName, "QuickTime Metadata Header", 3),
                new MetaDateTimeExtension(MetadataCaptions.QuickTime_Movie, MetaTypes.AttributeName, "QuickTime Movie Header", 3),
                new MetaDateTimeExtension(MetadataCaptions.QuickTime_Track, MetaTypes.AttributeName, "QuickTime Track Header", 3),
            };
        
        public static readonly Dictionary<string, MetadataInfoModel> DefaultMetadataInfos =
            new Dictionary<string, MetadataInfoModel>
            {
                {
                    MetadataCaptions.Exif_IFD0,
                    new MetadataInfoModel(MetadataCaptions.Exif_IFD0,
                        "Exif IFD0",
                        "Date/Time",
                        "yyyy:MM:dd HH:mm:ss"
                        )
                },
                {
                    MetadataCaptions.QuickTime_Metadata,
                    new MetadataInfoModel(MetadataCaptions.QuickTime_Metadata,
                        "QuickTime Metadata Header",
                        "Creation Date",
                        "ddd MMM dd HH:mm:ss K yyyy")
                },
                {
                    MetadataCaptions.QuickTime_Movie,
                    new MetadataInfoModel(MetadataCaptions.QuickTime_Movie,
                        "QuickTime Movie Header",
                        "Created",
                        "ddd MMM dd HH:mm:ss yyyy")
                },
                {
                    MetadataCaptions.QuickTime_Track,
                    new MetadataInfoModel(MetadataCaptions.QuickTime_Track,
                        "QuickTime Track Header",
                        "Created",
                        "ddd MMM dd HH:mm:ss yyyy")
                },
            };
    }
}