using System;
using QuestPDF.Drawing.SpacePlan;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    internal class AspectRatio : ContainerElement
    {
        public float Ratio { get; set; } = 1;
        public AspectRatioOption Option { get; set; } = AspectRatioOption.FitWidth;
        
        internal override ISpacePlan Measure(Size availableSpace)
        {
            if(Child == null)
                return new FullRender(Size.Zero);
            
            var size = GetTargetSize(availableSpace);
            
            if (size.Height > availableSpace.Height + Size.Epsilon)
                return new Wrap();
            
            if (size.Width > availableSpace.Width + Size.Epsilon)
                return new Wrap();
            
            return new FullRender(size);
        }

        internal override void Draw(ICanvas canvas, Size availableSpace)
        {
            if (Child == null)
                return;
            
            var size = GetTargetSize(availableSpace);
            Child?.Draw(canvas, size);
        }
        
        private Size GetTargetSize(Size availableSpace)
        {
            var spaceRatio = availableSpace.Width / availableSpace.Height;

            var fitHeight = new Size(availableSpace.Height * Ratio, availableSpace.Height) ;
            var fitWidth = new Size(availableSpace.Width, availableSpace.Width / Ratio);

            return Option switch
            {
                AspectRatioOption.FitWidth => fitWidth,
                AspectRatioOption.FitHeight => fitHeight,
                AspectRatioOption.FitArea => Ratio < spaceRatio ? fitHeight : fitWidth,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}