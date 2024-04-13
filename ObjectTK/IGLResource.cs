//
// GLResource.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.Reflection;
using MINNOVAA.ObjectTK.Exceptions;

namespace MINNOVAA.ObjectTK
{
    /// <summary>
    /// Represents an OpenGL resource.<br/>
    /// Must be disposed explicitly, otherwise a warning will be logged indicating a memory leak.<br/>
    /// Can be derived to inherit the dispose pattern.
    /// </summary>
    public interface IGLResource  : IDisposable
    {

        /// <summary>
        /// Gets a values specifying if this resource has already been disposed.
        /// </summary>
        bool IsDisposed { get; set; }

    }
}