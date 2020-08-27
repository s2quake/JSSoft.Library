// Released under the MIT License.
// 
// Copyright (c) 2018 Ntreev Soft co., Ltd.
// Copyright (c) 2020 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using JSSoft.Library.ObjectModel;
using System;
using System.IO;

namespace JSSoft.Library.IO.Virtualization.Memory
{
    public class MemoryFolder : CategoryBase<MemoryFile, MemoryFolder, MemoryFileCollection, MemoryFolderCollection, MemoryStorage>, IFolder
    {
        public MemoryFolder CreateFolder(string name)
        {
            return this.Container.AddNew(this, name);
        }

        public MemoryFile CreateFile(string name, Stream stream, long length)
        {
            var file = this.Context.Items.AddNew(this, name);

            var data = new byte[length];
            stream.Read(data, 0, (int)length);

            file.Size = length;
            file.Data = data;

            return file;
        }

        public void Rename(string name)
        {
            this.Name = name;
        }

        public void Delete()
        {
            this.Dispose();
        }

        public void MoveTo(string folderPath)
        {
            var parentFolder = this.Container[folderPath];
            this.Parent = parentFolder;
        }

        public DateTime ModifiedDateTime
        {
            get;
            set;
        }

        #region IFolder

        IFolder IFolder.CreateFolder(string name)
        {
            return this.CreateFolder(name);
        }

        IFile IFolder.CreateFile(string name, Stream stream, long length)
        {
            return this.CreateFile(name, stream, length);
        }

        IFolder IFolder.Parent => this.Parent;

        IContainer<IFolder> IFolder.Folders => this.Categories;

        IContainer<IFile> IFolder.Files => this.Items;

        IStorage IFolder.Storage => this.Context;

        string IFileSystem.Path => this.Path;

        #endregion
    }
}
