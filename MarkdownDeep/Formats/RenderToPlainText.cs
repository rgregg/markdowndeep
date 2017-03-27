using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownDeep.Formats
{
    public class RenderToPlainText : FormatRenderer
    {
        public override void Render(Block block, Markdown m, StringBuilder b)
        {
            switch (block.blockType)
            {
                case BlockType.Blank:
                    return;

                case BlockType.p:
                case BlockType.span:
                    m.SpanFormatter.FormatPlain(b, block.buf, block.contentStart, block.contentLen);
                    b.Append(" ");
                    break;

                case BlockType.h1:
                case BlockType.h2:
                case BlockType.h3:
                case BlockType.h4:
                case BlockType.h5:
                case BlockType.h6:
                    m.SpanFormatter.FormatPlain(b, block.buf, block.contentStart, block.contentLen);
                    b.Append(" - ");
                    break;


                case BlockType.ol_li:
                case BlockType.ul_li:
                    b.Append("* ");
                    m.SpanFormatter.FormatPlain(b, block.buf, block.contentStart, block.contentLen);
                    b.Append(" ");
                    break;

                case BlockType.dd:
                    if (block.children != null)
                    {
                        b.Append("\n");
                        RenderChildren(block, m, b);
                    }
                    else
                        m.SpanFormatter.FormatPlain(b, block.buf, block.contentStart, block.contentLen);
                    break;

                case BlockType.dt:
                    {
                        if (block.children == null)
                        {
                            foreach (var l in block.Content.Split('\n'))
                            {
                                var str = l.Trim();
                                m.SpanFormatter.FormatPlain(b, str, 0, str.Length);
                            }
                        }
                        else
                        {
                            RenderChildren(block, m, b);
                        }
                        break;
                    }

                case BlockType.dl:
                    RenderChildren(block, m, b);
                    return;

                case BlockType.codeblock:
                    foreach (var line in block.children)
                    {
                        b.Append(line.buf, line.contentStart, line.contentLen);
                        b.Append(" ");
                    }
                    return;

                case BlockType.quote:
                case BlockType.li:
                case BlockType.ol:
                case BlockType.ul:
                case BlockType.HtmlTag:
                    RenderChildren(block, m, b);
                    return;
            }
        }


    }
}
