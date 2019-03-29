$(function () {
    // 行を追加
    addRow = function (elem) {
        if ($('.editing')[0]) {
            return;
        }
        var table = $(elem).closest('div').siblings('table');
        var newRow = $('#table-template').find('tbody >tr:last-child').clone();


        //newRow.find('td:nth-child(2) input[type="text"]').remove();

        newRow.attr('data-kpt-id', '0');
        newRow.find('td:not(:last-child) span').text('');
        newRow.find('td:not(:last-child) input').val('');

        newRow.find('td:not(:last-child) span').text('');
        newRow.find('td:not(:last-child) input').val('');
        newRow.find('td:last-child >.btn-group-1').hide();
        newRow.find('td:last-child >.btn-group-2').show();
        table.find('tbody').prepend(newRow);
        newRow.addClass('editing');
    };

    // 行を編集
    editRow = function (elem) {
        if ($('.editing')[0]) {
            return;
        }
        var editRow = $(elem).closest('tr');
        editRow.find('td:not(:last-child) span').hide();
        editRow.find('td:not(:last-child) input').show();
        editRow.find('td:last-child >.btn-group-1').hide();
        editRow.find('td:last-child >.btn-group-2').show();
        editRow.addClass('editing');
    };

    // 編集キャンセル
    cancelEdit = function () {
        if (jqxhr && jqxhr.readyState === 1) {
            // 連続通信の防止
            return false;
        }

        $('.loader').show();
        jqxhr = $.ajax({
            type: 'get',
            url: location.href,
            dataType: 'html'
        }).done(function (data, textStatus, jqXHR) {
            var contents = $('<div></div>').html(data).find('main >*');
            $('main').addClass('fixed-height');
            $('main').html(contents);
            $('main').removeClass('fixed-height');

            $('.loader').hide();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $('.loader').hide();
        });
    };

    // 登録
    upsertItem = function (elem) {
        var url = '/Kpt/UpsertItem';
        
        if (location.pathname !== '/Kpt') {
            url = url + '/' + getLastSeg();
        }
        postData(url, elem);
    };

    // 削除
    deleteItem = function (itemIndex) {

        var delItem = document.getElementById('DeleteItemIndex');
        delItem.value = itemIndex;

        var modeItem = document.getElementById('IsDeleteMode');
        modeItem.value = true;

        document.kptForm.submit();
    };

    // 行の順番入れ替え
    $('#sortdata').sortable();

    
    //- ボード全クリアサブミット
    $('#alldel').click(
        function () {
            //フォームのサブミット
            document.loginForm.action = "/Login/ResetBoard";
            document.loginForm.submit();
            console.log("submit");
        }
    );
    
});