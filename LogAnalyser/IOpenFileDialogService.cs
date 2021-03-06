﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="JCS">
//   JCSCopyright
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LogAnalyzer
{
    public interface IOpenFileDialogService
    {
        bool? ShowDialog();
        string FileName { get; set; }

        bool CheckFileExists { get; set; }

        bool Multiselect { get; set; }

        System.IO.Stream OpenFile();

        string Filter { get; set; }
    }
}