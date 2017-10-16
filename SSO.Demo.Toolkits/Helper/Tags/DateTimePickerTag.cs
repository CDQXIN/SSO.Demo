﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SSO.Demo.Toolkits.Enums;
using SSO.Demo.Toolkits.Extension;

namespace SSO.Demo.Toolkits.Helper.Tags
{
    /// <inheritdoc />
    /// <summary>
    /// 时间标签
    /// </summary>
    [HtmlTargetElement("datetimepicker", TagStructure = TagStructure.WithoutEndTag)]
    public class DateTimePickerTag : TagHelper
    {
        #region 初始化
        private const string ValueAttributeName = "value";
        private const string PlaceholderAttributeName = "placeholder";
        private const string DateTimeTypeAttributeName = "dateTimeType";
        private const string FormatAttributeName = "asp-format";
        private const string ForAttributeName = "asp-for";
        private const string DispalyAttributeName = "input-display";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(ValueAttributeName)]
        public object Value { get; set; }

        [HtmlAttributeName(PlaceholderAttributeName)]
        public string Placeholder { get; set; }

        [HtmlAttributeName(DateTimeTypeAttributeName)]
        public ELayerUiDateTimeType DateTimeType { get; set; }

        [HtmlAttributeName(FormatAttributeName)]
        public string Format { get; set; }

        [HtmlAttributeName(DispalyAttributeName)]
        public EInputDisplay InputDisplay { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlGenerator _generator;
        public DateTimePickerTag(IHtmlGenerator generator)
        {
            _generator = generator;
        }
        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.ThrowIfNull();
            output.ThrowIfNull();

            var modelExplorer = For.ModelExplorer;
            var attributes = new Dictionary<string, object>
            {
                {PlaceholderAttributeName,Placeholder??modelExplorer.Metadata.Description??modelExplorer.Metadata.DisplayName },
                {"class", "layui-input"}
            };
            if (modelExplorer.Metadata != null && modelExplorer.Metadata.IsRequired)
                attributes["required"] = "required";

            var value = modelExplorer.Model ?? Value;
            if (value is DateTime)
            {
                value = ((DateTime)value).ToString(FormatByType(DateTimeType));
            }
            var inputTagBuilder = _generator.GenerateTextBox(ViewContext,
                modelExplorer,
                For.Name,
                value,
                Format,
                attributes);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(inputTagBuilder);
            output.Attributes.Add("class", InputDisplay.GetDisplayName());

            var dateTimeTypeStr = DateTimeType.ToString();
            var idStr = NameAndIdProvider.CreateSanitizedId(ViewContext, For.Name,
                _generator.IdAttributeDotReplacement);
            output.PostElement.SetHtmlContent(string.Format(@"<script type='text/javascript'>
             layui.use('laydate',
                    function () {{
                        var laydate = layui.laydate;
                        laydate.render({{
                            elem: '#{0}',
                            type: '{1}'
                        }});
                    }});
            </script>", idStr, dateTimeTypeStr));
        }

        private static string FormatByType(ELayerUiDateTimeType layerUiDateTimeType)
        {
            string typeStr;
            switch (layerUiDateTimeType)
            {
                case ELayerUiDateTimeType.date:
                    typeStr = "yyyy-MM-dd";
                    break;
                case ELayerUiDateTimeType.month:
                    typeStr = "yyyy-MM";
                    break;
                case ELayerUiDateTimeType.year:
                    typeStr = "yyyy";
                    break;
                case ELayerUiDateTimeType.time:
                    typeStr = "HH:mm:ss";
                    break;
                default:
                    typeStr = "yyyy-MM-dd HH:mm:ss";
                    break;
            }
            return typeStr;
        }
    }
}
