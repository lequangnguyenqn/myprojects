/*Description
1. focus: we remove watermark if we text something
2. blur: when moving out, watermark is on if there is no text in the box; otherwise hide it
3. keydown: when enter button is press, let search. We call fucntion "dosearch"
*/
function SearchHelper(searchBox, btnSearch, telerikModelGrid) {
    this.Init(searchBox, btnSearch, telerikModelGrid);
};
SearchHelper.prototype = function () {
    var Init = function (searchBox, btnSearch, telerikModelGrid) {
        this._searchTextBox = searchBox;
        this._btnSearch = btnSearch;
        this._modelGrid = telerikModelGrid;
        var me = this;

        this._searchTextBox.focus(function () {
            $(this).filter(function () {
                return $(this).val() == "" || $(this).val() == "Nhập vào từ khóa tìm kiếm..."
            }).removeClass("watermarkOn").val("");
        });
        this._searchTextBox.blur(function () {
            $(this).filter(function () {
                return $(this).val() == ""
            }).addClass("watermarkOn").val("Nhập vào từ khóa tìm kiếm...");
        });

        this._btnSearch.click(function () {
            me.DoSearch();
            return false;
        });

        this._searchTextBox.keydown(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                me.DoSearch();
                return false;
            }
        });
    };

    var DoSearch = function () {
        var textSearch = new Helper().stripTagsOff(this._searchTextBox.val());
        this._searchTextBox.val(textSearch);
        if (textSearch == "Nhập vào từ khóa tìm kiếm...")
            textSearch = "";
        if (this._modelGrid) {
            //rebind the grid
            this._modelGrid.data('tGrid').rebind({ search: textSearch });
        }
    };

    return {
        Init: Init,
        DoSearch: DoSearch
    };

} ();

