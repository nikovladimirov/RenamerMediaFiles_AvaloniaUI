namespace RenamerMediaFiles.Helpers
{
    public enum DateSource
    {
        LastWriteTime,
        Exif_IFD0,
        Exif_SubIFD,
        File,
        QuickTime_Metadata_Header,
        QuickTime_Movie_Header
    }
}