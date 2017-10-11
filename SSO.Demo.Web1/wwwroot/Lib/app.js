﻿jQuery.extend({
    openLayer: function (url, params, title) {
        var indexx;
        layui.use('layer',
            function () {
                var $ = layui.$;
                var layer = layui.layer;

                var loadHtml = $.ajax({
                    url: url,
                    type: "Get",
                    async: false,
                    data: params
                }).responseText;

                var layerIndex = layer.open({
                    type: 1,
                    skin: 'layui-layer-rim', //加上边框
                    content: loadHtml,
                    title: title
                });

                indexx = layer.getFrameIndex(layerIndex); //获取窗口索引

                layer.iframeAuto(index);
            });

        return indexx;
    },
    confirmDelete: function (url, params,reload) {
        layer.confirm('是否确认删除？',
            function (index) {
                $.post(url,
                    params,
                    function (result) {
                        if (result.success) {
                            reload();
                            layer.close(index);
                            layer.msg(result.message);
                        } else
                            layer.msg(result.message);
                    });
            });
    }
});

String.prototype.getBytesLength = function () {
    return this.replace(/[^\x00-\xff]/gi, "--").length;
}

jQuery.fn.extend({
    formSerialize: function () {
        var data = {};
        $('input,select,textarea', this).each(function () {
            var element = $(this);
            if (!element.prop("disabled") && element.attr('name')) {
                var key = element.attr('name');
                data[key] = element.val();
            }
        });
        return data;
    }
});

$.validator.addMethod("charlength", function (value, element, params) {
    if (this.optional(element)) {
        return true;
    }
    var maxlength = parseInt(params.maxlength);

    return value.getBytesLength() <= maxlength;
});

$.validator.unobtrusive.adapters.add("charlength", ["maxlength"], function (options) {
    options.rules['charlength'] = {
        maxlength: options.params.maxlength
    };
    options.messages['charlength'] = options.message;
});

$.validator.addMethod("numberminvalue", function (value, element, params) {
    if (this.optional(element)) {
        return true;
    }
    var minvalue = parseFloat(params.minvalue);

    return value >= minvalue;
});

$.validator.unobtrusive.adapters.add("numberminvalue", ["minvalue"], function (options) {
    options.rules['numberminvalue'] = {
        minvalue: options.params.minvalue
    };
    options.messages['numberminvalue'] = options.message;
});