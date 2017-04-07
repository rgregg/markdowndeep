using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownDeep.Formats
{
    public abstract class FormatRenderer
    {
        public abstract void Render(Block block, Markdown m, StringBuilder b);

        public void RenderChildren(Block block, Markdown m, StringBuilder b)
        {
            foreach (var child in block.children)
            {
                Render(child, m, b);
            }
        }
    }
}
