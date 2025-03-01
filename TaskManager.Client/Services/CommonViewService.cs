﻿using Prism.Mvvm;
using System.IO;
using System.Net;
using System.Windows;
using TaskManager.Common.Models;

namespace TaskManager.Client.Services
{
    public class CommonViewService
    {
        private string _igameDialogFilterPattern = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        public Window CurrentOpenedWindow { get; private set; }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowActionResult(HttpStatusCode code, string message)
        {
            if (code == HttpStatusCode.OK)
                ShowMessage(code.ToString() + $"\n{message}");
            else
                ShowMessage(code.ToString() + "\n Error!!!");
        }

        public void OpenWindow(Window wnd, BindableBase viewModel)
        {
            CurrentOpenedWindow = wnd;
            wnd.DataContext = viewModel;
            wnd.ShowDialog();
        }

        public string GetFileFromDialog(string filter)
        {
            string filePath = String.Empty;

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = filter;

            bool? result = dialog.ShowDialog();

            if (result == true)
                filePath = dialog.FileName;

            return filePath;
        }

        public CommonModel SetPhotoForObject(CommonModel model)
        {
            string photoPath = GetFileFromDialog(_igameDialogFilterPattern);
            if (string.IsNullOrEmpty(photoPath) == false)
            {
                var photoBytes = File.ReadAllBytes(photoPath);
                model.Photo = photoBytes;
            }
            return model;
        }

    }
}
