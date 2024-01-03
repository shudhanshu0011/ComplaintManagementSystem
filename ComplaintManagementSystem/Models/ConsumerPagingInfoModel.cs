using X.PagedList;

namespace ConsumerListApp.Models
{
    public class ConsumerPagingInfoModel
    {
        public int? pageSize;
        public int sortBy;
        public string Search;
        public bool isAsc { get; set; }
        public StaticPagedList<ApiResModel> Consumers { get; set; }
    }
}

	//public async Task<IActionResult> Grid(string Flag, string FeederInput, string SubstaionCodeInput, string SubDivisionCodeInput, int sortby, bool isAsc = true, int? page = 1)
        //{

        //    if (page < 0)
        //    {
        //        page = 1;
        //    }

        //    ConsumerPagingInfoModel ConsumerPagingInfo = new ConsumerPagingInfoModel();
        //    var pageIndex = (page ?? 1) - 1;
        //    var pageSize = 5;

        //    string sortColumn;
        //    #region SortingColumn
        //    switch (sortby)
        //    {
        //        case 1:
        //            if (isAsc)
        //                sortColumn = "FeederCode";
        //            else
        //                sortColumn = "FeederCode Desc";
        //            break;

        //        case 2:
        //            if (isAsc)
        //                sortColumn = "Name";
        //            else
        //                sortColumn = "Name Desc";
        //            break;

        //        case 3:
        //            if (isAsc)
        //                sortColumn = "SubstationCode";
        //            else
        //                sortColumn = "SubstationCode Desc";
        //            break;
        //        default:
        //            sortColumn = "Name asc";
        //            break;

        //    }
        //    #endregion
        //    ApiFeederReqModel apiFeederReqModel = new ApiFeederReqModel();
        //    ApiSubReqModel apiSubReqModel = new ApiSubReqModel();
        //    if (Flag == "F")
        //    {
        //        apiFeederReqModel.flag = Flag;
        //        if (FeederInput == null)
        //        {
        //            return NoContent();
        //        }
        //        apiFeederReqModel.feeder_code = FeederInput.Split(',').ToList();

        //        var consumers = await _user.GetConsumerDataApi(Flag, apiFeederReqModel, apiSubReqModel, 0, 0);
        //        var consumersPagedList = new StaticPagedList<ApiResModel>(consumers, pageIndex + 1, pageSize, consumers.Count());
        //        ConsumerPagingInfo.Consumers = consumersPagedList;
        //        ConsumerPagingInfo.pageSize = page;
        //        ConsumerPagingInfo.sortBy = sortby;
        //        ConsumerPagingInfo.isAsc = isAsc;
        //        return View(ConsumerPagingInfo);

        //    }
        //    else if (Flag == "S")
        //    {

        //        apiSubReqModel.flag = Flag;
        //        if (SubstaionCodeInput == null || SubDivisionCodeInput == null)
        //        {
        //            return NoContent();
        //        }
        //        apiSubReqModel.substationcode = SubstaionCodeInput;
        //        apiSubReqModel.sdocode = SubDivisionCodeInput;

        //        var consumers = await _user.GetConsumerDataApi(Flag, apiFeederReqModel, apiSubReqModel, 0, 0);
        //        var consumersPagedList = new StaticPagedList<ApiResModel>(consumers, pageIndex + 1, pageSize, consumers.Count());
        //        ConsumerPagingInfo.Consumers = consumersPagedList;
        //        ConsumerPagingInfo.pageSize = page;
        //        ConsumerPagingInfo.sortBy = sortby;
        //        ConsumerPagingInfo.isAsc = isAsc;
        //        return View(ConsumerPagingInfo);
        //    }            
        //    return NoContent();
        //}





	@*@using (Html.BeginForm("Grid", "Demo", FormMethod.Get, new { @id = "frmSearch" }))
        {
            <div class="row">
                <div class="col-lg-12" style="margin-top: 25px">
                    @if(Model != null)
                    {
                        <table class="table table-bordered">
                            <thead>
                                <tr sortby="@Model.sortBy" pagesize="@Model.pageSize" isAsc="@Model.isAsc"></tr>
                            </thead>
                            <tbody>
                                @foreach (var product in Model.Consumers)
                                {
                                    <tr>
                                        <td>
                                            @product.feeder_code
                                        </td>
                                        <td>
                                            @product.name
                                        </td>
                                        <td>
                                            @product.substation_code
                                        </td>
                                    </tr>
                                }
                                <tr>
                                    <td colspan="3">
                                        @Html.PagedListPager((IPagedList)Model.Consumers,
                                page => Url.Action("Grid", new
                                {
                                page = page,
                                sortby = Model.sortBy,
                                Search =Model.Search
                                }),
                                PagedListRenderOptions.OnlyShowFivePagesAtATime)
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    }
                </div>
            </div>
        }*@