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
    }
}