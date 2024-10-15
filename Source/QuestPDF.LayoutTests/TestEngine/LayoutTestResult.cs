using QuestPDF.Infrastructure;

namespace QuestPDF.LayoutTests.TestEngine;

internal sealed class LayoutTestResult
{
    public Size PageSize { get; set; } = new();

    public DocumentLayout ActualLayout { get; set; } = new();
    public DocumentLayout ExpectedLayout { get; set; } = new();

    public sealed class DocumentLayout
    {
        public ICollection<PageLayout> Pages { get; set; } = [];
        public bool GeneratesInfiniteLayout { get; set; } = new();
    }

    public sealed class PageLayout
    {
        public Size RequiredArea { get; set; } = new();
        public ICollection<MockLayoutPosition> Mocks { get; set; } = [];
    }

    public sealed class MockLayoutPosition
    {
        public string MockId { get; set; } = string.Empty;
        public Position Position { get; set; } = new();
        public Size Size { get; set; } = new();
    }
}

internal static class LayoutTestResultHelpers
{
    public static IEnumerable<(LayoutTestResult.MockLayoutPosition Below, LayoutTestResult.MockLayoutPosition Above)> GetOverlappingItems(this ICollection<LayoutTestResult.MockLayoutPosition> items)
    {
        for (var i = 0; i < items.Count; i++)
        {
            for (var j = i + 1; j < items.Count; j++)
            {
                var beforeChild = items.ElementAt(i);
                var afterChild = items.ElementAt(j);

                var beforeBoundingBox = BoundingBox.From(beforeChild.Position, beforeChild.Size);
                var afterBoundingBox = BoundingBox.From(afterChild.Position, afterChild.Size);

                var intersection = BoundingBoxExtensions.Intersection(beforeBoundingBox, afterBoundingBox);

                if (intersection == null)
                    continue;

                yield return (beforeChild, afterChild);
            }
        }
    }
}