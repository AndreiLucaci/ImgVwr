using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using ImgVwr.Models;

namespace ImgVwr
{
    public class Engine
    {
        private string _currentFile;
        private string _workingDirectory;
        private Stream _bitmapImageStream;

        private int _currentIndex = -1;

        private List<string> _availablePhotos = new List<string>();

        private readonly string[] _validExtensions = {"jpg", "png"};

        public Engine(string currentFile = null)
        {
            _currentFile = currentFile;

            SetCurrentWorkingDirectory();
        }

        public ImageModel Next()
        {
            if (_availablePhotos.Any())
            {
                if (_currentIndex == _availablePhotos.Count - 1)
                {
                    _currentIndex = -1;
                }
                _currentFile = _availablePhotos[++_currentIndex];
                return LoadImage(_currentFile);
            }
            return null;
        }

        public ImageModel Previous()
        {
            if (_availablePhotos.Any())
            {
                if (_currentIndex == 0)
                {
                    _currentIndex = _availablePhotos.Count;
                }
                _currentFile = _availablePhotos[--_currentIndex];
                return LoadImage(_currentFile);
            }
            return null;
        }

        public ImageModel LoadImage(string path)
        {
            ClearBitmapStream();

            var bitMap = new BitmapImage();
            _bitmapImageStream = new FileStream(path, FileMode.Open);

            bitMap.BeginInit();
            bitMap.CacheOption = BitmapCacheOption.None;
            bitMap.StreamSource = _bitmapImageStream;
            bitMap.EndInit();

            bitMap.Freeze();

            return new ImageModel
            {
                ImageSource = bitMap,
                Path = path,
                Name = Path.GetFileName(path)
            };
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(_currentFile) || _availablePhotos.Any();
        }

        private void LoadPhotosFromCurrentDirectory()
        {
            var accumullator = new List<string>();

            foreach (var ext in _validExtensions)
            {
                accumullator.AddRange(Directory.GetFiles(_workingDirectory, $"*.{ext}"));
            }

            _availablePhotos = accumullator;
        }
        
        private void SetCurrentWorkingDirectory()
        {
            _workingDirectory = !string.IsNullOrEmpty(_currentFile)
                ? Path.GetDirectoryName(_currentFile)
                : Directory.GetCurrentDirectory();

            LoadPhotosFromCurrentDirectory();

            SetCurrentIndex();
        }

        private void SetCurrentIndex()
        {
            if (!string.IsNullOrEmpty(_currentFile) && _availablePhotos.Any())
            {
                _currentIndex = _availablePhotos.IndexOf(_currentFile);
            }
        }

        private void ClearBitmapStream()
        {
            if (_bitmapImageStream != null)
            {
                _bitmapImageStream.Close();
                _bitmapImageStream.Dispose();
                _bitmapImageStream = null;
            }
        }
    }
}
