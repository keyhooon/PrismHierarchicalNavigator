using MaterialDesignThemes.Wpf;

namespace CompositeContentNavigator
{
    public class ContentNavigatorOptions
    {



        public ContentNavigatorOptions()
        {

            HasRoot = false;
            RootDisplay = "";
            RootPackIcon = PackIconKind.Abc;
            ContentRegionName  = "ContentRegion";
            ContentMapRegionName  = "ContentMapRegion";
            HeaderRegionName = "HeaderRegion";
            ToolbarRegionName = "ToolbarRegion";

    }

        public bool HasRoot { get; set; }

        public string RootDisplay { get; set; }
        public PackIconKind RootPackIcon { get; set; }

        public string ContentRegionName { get; set; } 

        public string ContentMapRegionName { get; set; } 

        public string HeaderRegionName { get; set; }

        public string ToolbarRegionName { get; set; }

    }
}
