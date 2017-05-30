using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownDeep.Formats
{
    public class RenderToHtml : FormatRenderer
    {
        public override void Render(Block block, Markdown m, StringBuilder b)
        {
            switch (block.blockType)
            {
                case BlockType.Blank:
                    return;

                case BlockType.p:
                    m.SpanFormatter.FormatParagraph(b, block.buf, block.contentStart, block.contentLen);
                    break;

                case BlockType.span:
                    m.SpanFormatter.Format(b, block.buf, block.contentStart, block.contentLen);
                    b.Append("\n");
                    break;

                case BlockType.h1:
                case BlockType.h2:
                case BlockType.h3:
                case BlockType.h4:
                case BlockType.h5:
                case BlockType.h6:
                    if (m.ExtraMode)
                    {
                        b.Append("<" + block.blockType.ToString());
                        string id = block.ResolveHeaderID(m);
                        if (!String.IsNullOrEmpty(id))
                        {
                            b.Append(" id=\"");
                            b.Append(id);
                            b.Append("\">");
                        }
                        else
                        {
                            b.Append(">");
                        }
                    }
                    else
                    {
                        b.Append("<" + block.blockType.ToString() + ">");
                    }
                    m.SpanFormatter.Format(b, block.buf, block.contentStart, block.contentLen);
                    b.Append("</" + block.blockType.ToString() + ">\n");
                    break;

                case BlockType.hr:
                    b.Append("<hr />\n");
                    return;

                case BlockType.user_break:
                    return;

                case BlockType.ol_li:
                case BlockType.ul_li:
                    b.Append("<li>");
                    m.SpanFormatter.Format(b, block.buf, block.contentStart, block.contentLen);
                    b.Append("</li>\n");
                    break;

                case BlockType.dd:
                    b.Append("<dd>");
                    if (block.children != null)
                    {
                        b.Append("\n");
                        RenderChildren(block, m, b);
                    }
                    else
                        m.SpanFormatter.Format(b, block.buf, block.contentStart, block.contentLen);
                    b.Append("</dd>\n");
                    break;

                case BlockType.dt:
                    {
                        if (block.children == null)
                        {
                            foreach (var l in block.Content.Split('\n'))
                            {
                                b.Append("<dt>");
                                m.SpanFormatter.Format(b, l.Trim());
                                b.Append("</dt>\n");
                            }
                        }
                        else
                        {
                            b.Append("<dt>\n");
                            RenderChildren(block, m, b);
                            b.Append("</dt>\n");
                        }
                        break;
                    }

                case BlockType.dl:
                    b.Append("<dl>\n");
                    RenderChildren(block, m, b);
                    b.Append("</dl>\n");
                    return;

                case BlockType.html:
                    b.Append(block.buf, block.contentStart, block.contentLen);
                    return;

                case BlockType.unsafe_html:
                    if (m.EncodeUnsafeHtml)
                    {
                        m.HtmlEncode(b, block.buf, block.contentStart, block.contentLen);
                    }
                    return;

                case BlockType.codeblock:
                    if (m.FormatCodeBlock != null)
                    {
                        var sb = new StringBuilder();
                        foreach (var line in block.children)
                        {
                            m.HtmlEncodeAndConvertTabsToSpaces(sb, line.buf, line.contentStart, line.contentLen);
                            sb.Append("\n");
                        }
                        b.Append(m.FormatCodeBlock(m, sb.ToString()));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty((block.CodeLanguage)))
                            b.AppendFormat("<pre><code class=\"{0}\">", block.CodeLanguage);
                        else
                            b.Append("<pre><code>");

                        foreach (var line in block.children)
                        {
                            m.HtmlEncodeAndConvertTabsToSpaces(b, line.buf, line.contentStart, line.contentLen);
                            b.Append("\n");
                        }
                        b.Append("</code></pre>\n\n");
                    }
                    return;

                case BlockType.quote:
                    b.Append("<blockquote>\n");
                    RenderChildren(block, m, b);
                    b.Append("</blockquote>\n");
                    return;

                case BlockType.li:
                    b.Append("<li>\n");
                    RenderChildren(block, m, b);
                    b.Append("</li>\n");
                    return;

                case BlockType.ol:
                    if (!m.RespectOrderedListStartValues || block.OlStart <= 1)
                    {
                        b.Append("<ol>\n");
                    }
                    else
                    {
                        b.Append("<ol start=\"" + block.OlStart + "\">\n");
                    }
                    RenderChildren(block, m, b);
                    b.Append("</ol>\n");
                    return;

                case BlockType.ul:
                    b.Append("<ul>\n");
                    RenderChildren(block, m, b);
                    b.Append("</ul>\n");
                    return;

                case BlockType.HtmlTag:
                    var tag = (HtmlTag)block.data;

                    // Prepare special tags
                    var name = tag.name.ToLowerInvariant();
                    if (name == "a")
                    {
                        m.OnPrepareLink(tag);
                    }
                    else if (name == "img")
                    {
                        m.OnPrepareImage(tag, m.RenderingTitledImage);
                    }

                    tag.RenderOpening(b);
                    b.Append("\n");
                    RenderChildren(block, m, b);
                    tag.RenderClosing(b);
                    b.Append("\n");
                    return;

                case BlockType.Composite:
                case BlockType.footnote:
                    RenderChildren(block, m, b);
                    return;

                case BlockType.table_spec:
                    ((TableSpec)block.data).Render(m, b);
                    break;

                case BlockType.p_footnote:
                    b.Append("<p>");
                    if (block.contentLen > 0)
                    {
                        m.SpanFormatter.Format(b, block.buf, block.contentStart, block.contentLen);
                        b.Append("&nbsp;");
                    }
                    b.Append((string)block.data);
                    b.Append("</p>\n");
                    break;

                default:
                    b.Append("<" + block.blockType.ToString() + ">");
                    m.SpanFormatter.Format(b, block.buf, block.contentStart, block.contentLen);
                    b.Append("</" + block.blockType.ToString() + ">\n");
                    break;
            }
        }

    }
}
