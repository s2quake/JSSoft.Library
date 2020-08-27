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
// 
// Forked from https://github.com/NtreevSoft/Ntreev.Library
// Namespaces and files starting with "Ntreev" have been renamed to "JSSoft".

using System;
using System.Collections.Generic;
using System.Linq;

namespace JSSoft.Library
{
    public static class NameUtility
    {
        public static string GenerateNewName(string name, IEnumerable<string> existedNames)
        {
            var index = 1;
            var newName = name + index++;

            while (existedNames.Contains(newName) == true)
            {
                newName = name + index++;
            }

            return newName;
        }

        public static string GenerateNewName(string name, IEnumerable<string> existedNames, Func<string, int, string> generator)
        {
            var index = 1;
            var newName = generator(name, index++);

            while (existedNames.Contains(newName) == true)
            {
                newName = generator(name, index++);
            }

            return newName;
        }

        public static string GetParentName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Contains('.') == true)
            {
                var items = StringUtility.Split(name, '.');
                return string.Join(".", items.Take(items.Length - 1));
            }
            return string.Empty;
        }

        public static string GetName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Contains('.') == true)
            {
                return StringUtility.Split(name, '.').Last();
            }
            return name;
        }
    }
}
