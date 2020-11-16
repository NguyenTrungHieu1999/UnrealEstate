namespace UnrealEstate.ViewModels.FilterModel
{
    public class FilterListingModel
    {
        public decimal? MaxPrice { get; set; }

        public int? MaxAge { get; set; }

        public double? MinSize { get; set; }

        public bool? Sold { get; set; }

        public string Address { get; set; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }
    }
}
