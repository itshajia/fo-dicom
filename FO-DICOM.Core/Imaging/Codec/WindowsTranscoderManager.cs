﻿// Copyright (c) 2012-2019 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

using System;
using System.Linq;
using System.Reflection;

namespace FellowOakDicom.Imaging.Codec
{

    /// <summary>
    /// Implementation of <see cref="TranscoderManager"/> for Universal Windows Platform applications.
    /// </summary>
    public sealed class WindowsTranscoderManager : TranscoderManager
    {

        #region CONSTRUCTORS

        /// <summary>
        /// Initializes an instance of <see cref="WindowsTranscoderManager"/>.
        /// </summary>
        public WindowsTranscoderManager()
        {
            LoadCodecs(null, null);
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Implementation of method to load codecs from assembly(ies) at the specified <paramref name="path"/> and 
        /// with the specified <paramref name="search"/> pattern.
        /// </summary>
        /// <param name="path">Directory path to codec assemblies.</param>
        /// <param name="search">Search pattern for codec assemblies.</param>
        public override void LoadCodecs(string path, string search)
        {
            var assembly = typeof(WindowsTranscoderManager).GetTypeInfo().Assembly;
            var types =
                assembly.DefinedTypes.Where(
                    ti => ti.IsClass && !ti.IsAbstract && ti.ImplementedInterfaces.Contains(typeof(IDicomCodec)));

            foreach (var ti in types)
            {
                var codec = (IDicomCodec)Activator.CreateInstance(ti.AsType());
                Codecs[codec.TransferSyntax] = codec;
            }
        }

        #endregion
    }
}
