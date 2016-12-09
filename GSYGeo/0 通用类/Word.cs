﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Word;
using MSWord = Microsoft.Office.Interop.Word;

namespace GSYGeo
{
    /// <summary>
    /// 操作Word文档的类
    /// </summary>
    public class Word
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Word()
        {
            App = new MSWord.Application();
            Doc = App.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
        }

        /// <summary>
        /// 空值
        /// </summary>
        object Nothing = Missing.Value;

        /// <summary>
        /// 属性，Word应用
        /// </summary>
        public MSWord.Application App { get; set; }

        /// <summary>
        /// 属性，Word文档
        /// </summary>
        public MSWord.Document Doc { get; set; }

        /// <summary>
        /// 属性，Word文档中的表格
        /// </summary>
        public MSWord.Tables Tables
        {
            get
            {
                return Doc.Tables;
            }
        }

        /// <summary>
        /// 保存并退出Word，保存为97-2003格式
        /// </summary>
        /// <param name="_path">保存路径</param>
        public void SaveAndQuit(object _path)
        {
            object format = MSWord.WdSaveFormat.wdFormatDocument97;
            Doc.SaveAs2(ref _path, ref format, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            Doc.Close(ref Nothing, ref Nothing, ref Nothing);
            App.Quit(ref Nothing, ref Nothing, ref Nothing);
        }

        /// <summary>
        /// 添加新表格
        /// </summary>
        /// <param name="_title">表格标题</param>
        /// <param name="_countRows">表格行数</param>
        /// <param name="_countColumns">表格列数</param>
        /// <returns>新添加的表格</returns>
        public MSWord.Table AddTable(string _title,int _countRows,int _countColumns)
        {
            // 填写表格标题
            Doc.Paragraphs.Last.Range.Text = _title;
            Doc.Paragraphs.Last.Range.Font.Bold = 1;
            Doc.Paragraphs.Last.Range.Font.Size = 12;
            App.Selection.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            object unite = MSWord.WdUnits.wdStory;
            App.Selection.EndKey(ref unite, ref Nothing);

            // 定义表格对象
            MSWord.Table table = Tables.Add(App.Selection.Range, _countRows, _countColumns, ref Nothing, ref Nothing);
            return table;
        }

        /// <summary>
        /// 添加标贯/动探统计表格
        /// </summary>
        /// <param name="_nTestStatistic">标贯/动探统计数据</param>
        /// <returns></returns>
        public MSWord.Table AddNTestStatisticTable(List<StatisticNTest> _nTestStatistic)
        {
            // 填写表格标题
            Doc.Paragraphs.Last.Range.Text = "表3 标贯/动探锤击数统计表";
            Doc.Paragraphs.Last.Range.Font.Bold = 1;
            Doc.Paragraphs.Last.Range.Font.Size = 12;
            App.Selection.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            object unite = MSWord.WdUnits.wdStory;
            App.Selection.EndKey(ref unite, ref Nothing);

            // 定义表格对象
            MSWord.Table table = Tables.Add(App.Selection.Range, _nTestStatistic.Count + 1, 11, ref Nothing, ref Nothing);
            
            // 填充行标题
            string[] rowheader = new string[] { "层号", "岩土名称", "类型", "统计数", "最大值", "最小值", "平均值", "标准差", "变异系数", "统计修正系数", "标准值" };
            for(int i = 1; i <= table.Columns.Count; i++)
            {
                table.Cell(1, i).Range.Text = rowheader[i - 1];
            }

            // 设置文档格式
            Doc.PageSetup.LeftMargin = 50F;
            Doc.PageSetup.RightMargin = 50F;

            // 设置表格格式
            table.Select();
            App.Selection.Tables[1].Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
            table.Rows[1].Range.Bold = 1;
            for(int i = 2; i <= table.Rows.Count; i++)
            {
                table.Rows[i].Range.Bold = 0;
                table.Columns[7].Cells[i].Range.Bold = 1;
                table.Columns[11].Cells[i].Range.Bold = 1;
            }
            table.Range.Font.Size = 10.5F;
            table.Range.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Range.Cells.VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Borders.OutsideLineStyle = MSWord.WdLineStyle.wdLineStyleDouble;
            table.Borders.InsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;
            float[] columnWidth = new float[] { 35, 80, 35, 45, 45, 45, 45, 45, 35, 45, 45 };
            for(int i = 1; i <= table.Columns.Count; i++)
            {
                table.Columns[i].Width = columnWidth[i - 1];
            }

            // 填充标贯/动探数据
            for(int i = 2; i <= table.Rows.Count; i++)
            {
                table.Cell(i, 1).Range.Text = _nTestStatistic[i - 2].Layer;
                table.Cell(i, 2).Range.Text = _nTestStatistic[i - 2].Name;
                table.Cell(i, 3).Range.Text = _nTestStatistic[i - 2].Type.ToString();
                table.Cell(i, 4).Range.Text = _nTestStatistic[i - 2].Count.ToString("0");
                table.Cell(i, 5).Range.Text = _nTestStatistic[i - 2].Max.ToString("0");
                table.Cell(i, 6).Range.Text = _nTestStatistic[i - 2].Min.ToString("0");
                table.Cell(i, 7).Range.Text = _nTestStatistic[i - 2].Average.ToString("0.0");
                table.Cell(i, 8).Range.Text = _nTestStatistic[i - 2].StandardDeviation.ToString() == "-0.19880205" ? "/" : _nTestStatistic[i - 2].StandardDeviation.ToString("0.0");
                table.Cell(i, 9).Range.Text = _nTestStatistic[i - 2].VariableCoefficient.ToString() == "-0.19880205" ? "/" : _nTestStatistic[i - 2].VariableCoefficient.ToString("0.00");
                table.Cell(i, 10).Range.Text = _nTestStatistic[i - 2].CorrectionCoefficient.ToString() == "-0.19880205" ? "/" : _nTestStatistic[i - 2].CorrectionCoefficient.ToString("0.00");
                table.Cell(i, 11).Range.Text = _nTestStatistic[i - 2].StandardValue.ToString() == "-0.19880205" ? "/" : _nTestStatistic[i - 2].StandardValue.ToString("0.0");

            }

            //返回
            return table;
        }
        
        public MSWord.Table AddPsStatisticTable(List<StatisticCPT> _cptStatistic)
        {
            // 填写表格标题
            Doc.Paragraphs.Last.Range.Text = "表2 静力触探摩阻力统计表";
            Doc.Paragraphs.Last.Range.Font.Bold = 1;
            Doc.Paragraphs.Last.Range.Font.Size = 12;
            App.Selection.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            object unite = MSWord.WdUnits.wdStory;
            App.Selection.EndKey(ref unite, ref Nothing);

            // 定义表格对象
            MSWord.Table table = Tables.Add(App.Selection.Range, _cptStatistic.Count + 1, 10, ref Nothing, ref Nothing);

            // 填充行标题
            string[] rowheader = new string[] { "层号", "岩土名称", "统计数", "最大值", "最小值", "平均值", "标准差", "变异系数", "统计修正系数", "标准值" };
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                table.Cell(1, i).Range.Text = rowheader[i - 1];
            }

            // 设置文档格式
            Doc.PageSetup.LeftMargin = 50F;
            Doc.PageSetup.RightMargin = 50F;

            // 设置表格格式
            table.Select();
            App.Selection.Tables[1].Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
            table.Rows[1].Range.Bold = 1;
            for (int i = 2; i <= table.Rows.Count; i++)
            {
                table.Rows[i].Range.Bold = 0;
                table.Columns[6].Cells[i].Range.Bold = 1;
                table.Columns[10].Cells[i].Range.Bold = 1;
            }
            table.Range.Font.Size = 10.5F;
            table.Range.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Range.Cells.VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Borders.OutsideLineStyle = MSWord.WdLineStyle.wdLineStyleDouble;
            table.Borders.InsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;
            float[] columnWidth = new float[] { 35, 80, 45, 45, 45, 45, 45, 35, 45, 45 };
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                table.Columns[i].Width = columnWidth[i - 1];
            }

            // 填充摩阻力数据
            for (int i = 2; i <= table.Rows.Count; i++)
            {
                table.Cell(i, 1).Range.Text = _cptStatistic[i - 2].Layer;
                table.Cell(i, 2).Range.Text = _cptStatistic[i - 2].Name;
                table.Cell(i, 3).Range.Text = _cptStatistic[i - 2].Count.ToString("0");
                table.Cell(i, 4).Range.Text = _cptStatistic[i - 2].Max.ToString("0.00");
                table.Cell(i, 5).Range.Text = _cptStatistic[i - 2].Min.ToString("0.00");
                table.Cell(i, 6).Range.Text = _cptStatistic[i - 2].Average.ToString("0.00");
                table.Cell(i, 7).Range.Text = _cptStatistic[i - 2].StandardDeviation.ToString() == "-0.19880205" ? "/" : _cptStatistic[i - 2].StandardDeviation.ToString("0.00");
                table.Cell(i, 8).Range.Text = _cptStatistic[i - 2].VariableCoefficient.ToString() == "-0.19880205" ? "/" : _cptStatistic[i - 2].VariableCoefficient.ToString("0.00");
                table.Cell(i, 9).Range.Text = _cptStatistic[i - 2].CorrectionCoefficient.ToString() == "-0.19880205" ? "/" : _cptStatistic[i - 2].CorrectionCoefficient.ToString("0.00");
                table.Cell(i, 10).Range.Text = _cptStatistic[i - 2].StandardValue.ToString() == "-0.19880205" ? "/" : _cptStatistic[i - 2].StandardValue.ToString("0.00");
            }

            //返回
            return table;
        }
    }
}
