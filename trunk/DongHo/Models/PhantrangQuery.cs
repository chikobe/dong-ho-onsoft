using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DongHo.Models
{
    public class PhantrangQuery
    {
        public static string PhanTrangQuery(int numItems, int curpage, int numOfNews, string url)
        {
            //int numItems = 10; // so san pham tren 1 trang
            //int numOfNews = 0; // tong so san pham da goi len duoc tu db
            int numpages = 0;   //tong so trang co duoc khi tien hanh phan trang
            string showpage = "";
            numpages = numOfNews / numItems;
            if (numOfNews % numItems > 0)
            {
                numpages += 1;
            }
            if (curpage < 0)
            {
                curpage = 0;
            }
            if (numOfNews > 0)
            {
                showpage = "<table border='0' class=\"tablePaging\"><tr>";
                if (numpages == 1)
                {
                    showpage += "<td><span>1</span></td>";
                }
                else if (numpages < 10)
                {
                    if (curpage == 0)
                    {
                        for (int i = 0; i < numpages; i++)
                        {
                            if (i == curpage)
                            {
                                showpage += "<td><span>" + (i + 1) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (i + 1) + "\">" + (i + 1) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                    else if (curpage == numpages - 1)
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage) + "\"><</a></td>";
                        for (int i = 0; i < numpages; i++)
                        {
                            if (i == numpages - 1)
                            {
                                showpage += "<td><span>" + (curpage + 1) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (i + 1) + "\">" + (i + 1) + "</a></td>";
                            }
                        }
                    }
                    else if (numpages < curpage + 2)
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage) + "\"><</a></td>";
                        for (int i = 0; i < numpages; i++)
                        {
                            if (i == curpage)
                            {
                                showpage += "<td><span>" + (i + 1) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (i + 1) + "\">" + (i + 1) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                    else
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage) + "\"><</a></td>";
                        for (int i = 0; i < numpages; i++)
                        {
                            if (i == curpage)
                            {
                                showpage += "<td><span>" + (i + 1) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (i + 1) + "\">" + (i + 1) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                }
                else if (numpages >= 10)
                {
                    if (curpage == 0)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == curpage)
                            {
                                showpage += "<td><span>" + (i + 1) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (i + 1) + "\">" + (i + 1) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                    else if (curpage == numpages - 1)
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\"><</a></td>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 8)
                            {
                                showpage += "<td><span>" + (curpage + 1) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (numpages - 8 + i) + "\">" + (numpages - 8 + i) + "</a></td>";
                            }
                        }
                    }
                    else if (curpage == 1)
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage) + "\"><</a></td>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 1)
                            {
                                showpage += "<td><span>" + (i + 1) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (i + 1) + "\">" + (i + 1) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                    else if (numpages == curpage + 2)
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage) + "\"><</a></td>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 7)
                            {
                                showpage += "<td><span>" + (numpages - 8 + i) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (numpages - 8 + i) + "\">" + (numpages - 8 + i) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                    else if (numpages == curpage + 3)
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage) + "\"><</a></td>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 6)
                            {
                                showpage += "<td><span>" + (numpages - 8 + i) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (numpages - 8 + i) + "\">" + (numpages - 8 + i) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                    else if (numpages == curpage + 4)
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage) + "\"><</a></td>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 5)
                            {
                                showpage += "<td><span>" + (numpages - 8 + i) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (numpages - 8 + i) + "\">" + (numpages - 8 + i) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                    else if (numpages == curpage + 5)
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage) + "\"><</a></td>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 4)
                            {
                                showpage += "<td><span>" + (numpages - 8 + i) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (numpages - 8 + i) + "\">" + (numpages - 8 + i) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                    else if (numpages == curpage + 6)
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage) + "\"><</a></td>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 3)
                            {
                                showpage += "<td><span>" + (numpages - 8 + i) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (numpages - 8 + i) + "\">" + (numpages - 8 + i) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                    else
                    {
                        showpage += "<td><a href=\"" + url + "&page=1\"><<</a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + curpage + "\"><</a></td>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 2)
                            {
                                showpage += "<td><span>" + (curpage + 1) + "</span></td>";
                            }
                            else
                            {
                                showpage += "<td><a href=\"" + url + "&page=" + (curpage - 1 + i) + "\">" + (curpage - 1 + i) + "</a></td>";
                            }
                        }
                        showpage += "<td><a href=\"" + url + "&page=" + (curpage + 2) + "\">></a></td>";
                        showpage += "<td><a href=\"" + url + "&page=" + numpages + "\">>></a></td>";
                    }
                }
            }
            showpage += "</tr></table>";
            return showpage;
        }
    }
}