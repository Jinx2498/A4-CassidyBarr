using System.Collections.Generic;
using GameBrains.Actuators.Motion.Navigation.SearchGraph;

namespace GameBrains.Actuators.Motion.Navigation.SearchAlgorithms.CycleLimitedSearch
{
    public abstract class Search
    {
        protected Search(SearchAlgorithms searchAlgorithms, Node source)
        {
            SearchAlgorithms = searchAlgorithms;
            Source = source;
        }

        public SearchAlgorithms SearchAlgorithms { get; }

        public Node Source { get; }

        public List<Edge> Solution { get; protected set; }
        
        public abstract SearchResults DoAllCycleOfSearch();
        public abstract SearchResults DoOneCycleOfSearch();

        // Do cycle-limited search.
        public abstract SearchResults DoSearch(out int cyclesUsed, int cycles = int.MaxValue);

        // Extracts the list of edges in a path by working backwards from the current node.
        protected static List<Edge> ExtractPath(PathData current)
        {
            var path = new List<Edge>();

            while (current.edgeFromParent)
            {
                path.Add(current.edgeFromParent);
                current = current.parentPathData;
            }

            path.Reverse();
            return path;
        }
    }
}