﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors;
using SixLabors.Primitives;
using Yugen.Mosaic.Uwp.Models;

namespace Yugen.Mosaic.Uwp.Processors
{
    public sealed class GetTileAverageProcessor : IImageProcessor
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;

        public YugenColor MyColor;

        public GetTileAverageProcessor(int x, int y, int width, int height, YugenColor myColor)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;

            MyColor = myColor;
        }

        /// <inheritdoc/>
        public IImageProcessor<TPixel> CreatePixelSpecificProcessor<TPixel>(Image<TPixel> source, Rectangle sourceRectangle) where TPixel : struct, IPixel<TPixel>
        {
            return new GetTileAverageProcessor<TPixel>(this, source, sourceRectangle, _x, _y, _width, _height, MyColor);
        }
    }

    public class GetTileAverageProcessor<TPixel> : IImageProcessor<TPixel> where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// The source <see cref="Image{TPixel}"/> instance to modify
        /// </summary>
        private readonly Image<TPixel> Source;

        private int _x;
        private int _y;
        private int _width;
        private int _height;

        public YugenColor MyColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HlslGaussianBlurProcessor"/> class
        /// </summary>
        /// <param name="definition">The <see cref="HlslGaussianBlurProcessor"/> defining the processor parameters</param>
        /// <param name="source">The source <see cref="Image{TPixel}"/> for the current processor instance</param>
        /// <param name="sourceRectangle">The source area to process for the current processor instance</param>
        public GetTileAverageProcessor(GetTileAverageProcessor definition, Image<TPixel> source, Rectangle sourceRectangle, int x, int y, int width, int height, YugenColor myColor)
        {
            Source = source;

            _x = x;
            _y = y;
            _width = width;
            _height = height;

            MyColor = myColor;
        }


        /// <inheritdoc/>
        public void Apply()
        {
            int width = Source.Width;
            Image<TPixel> source = Source; // Avoid capturing this

            long aR = 0;
            long aG = 0;
            long aB = 0;

            for (int h = _y; h < _y + _height; h++)
            {
                var rowSpan = source.GetPixelRowSpan(h);

                for (int w = _x; w < _x + _width; w++)
                {
                    Rgba32 pixel = new Rgba32();
                    rowSpan[w].ToRgba32(ref pixel);

                    aR += pixel.R;
                    aG += pixel.G;
                    aB += pixel.B;
                }
            }

            aR /= width * _height;
            aG /= width * _height;
            aB /= width * _height;

            MyColor.R = aR;
            MyColor.G = aG;
            MyColor.B = aB;
        }

        /// <inheritdoc/>
        public void Dispose() { }
    }
}