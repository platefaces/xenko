﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System.Threading.Tasks;

using NUnit.Framework;

using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.UI.Controls;

namespace SiliconStudio.Xenko.UI.Tests.Regression
{
    /// <summary>
    /// Class for rendering tests on the <see cref="ImageElement"/> 
    /// </summary>
    public class ImageTest : UITestGameBase
    {
        public ImageTest()
        {
            CurrentVersion = 3;
        }

        protected override async Task LoadContent()
        {
            await base.LoadContent();

            UIComponent.RootElement = new ImageElement { Source = new Sprite(Asset.Load<Texture>("uv"))};
        }

        protected override void RegisterTests()
        {
            base.RegisterTests();

            FrameGameSystem.TakeScreenshot();
        }

        [Test]
        public void RunImageTest()
        {
            RunGameTest(new ImageTest());
        }

        /// <summary>
        /// Launch the Image test.
        /// </summary>
        public static void Main()
        {
            using (var game = new ImageTest())
                game.Run();
        }
    }
}
