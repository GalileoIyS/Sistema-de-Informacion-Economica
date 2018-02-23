<%@ Page Language="C#" MasterPageFile="~/masterpages/NormalPage.master"
    AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Culture="auto" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContentPlaceHolder">
</asp:Content>
<asp:Content ID="CenterContent" runat="server" ContentPlaceHolderID="EntirePagePlaceHolder">
    <!-- Header -->
    <header>
        <div class="container">
            <div class="row">
                <div class="col-lg-6">
                    <div class="intro-text">
                        <span class="name">BUILDING KNOWLEDGE THROUGH DATA SHARING</span>
                        <hr class="star-light" />
                        <span class="skills">Search shared dropkeys or add your own that are best suited for your business. Use formulas to combine them and set in graphs to analyze the results.</span>
                        <div class="margin-top-10">
                            <asp:LoginView ID="StartNowKPIBoard" runat="server">
                                <AnonymousTemplate>
                                    <a href="#" class="btn btn-start login-trigger" data-toggle="modal" data-target="#myModal">
                                        <div><span class="glyphicon glyphicon-user"></span>&nbsp;Try it now</div>
                                        <div><span class="small">It's completely free!</span></div>
                                    </a>
                                </AnonymousTemplate>
                                <LoggedInTemplate>
                                    <a runat="server" href="~/registrado/desktop.aspx" class="btn btn-start">
                                        <div><i class="fa fa-play"></i>&nbsp;Start now!</div>
                                    </a>
                                </LoggedInTemplate>
                            </asp:LoginView>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <img class="img-responsive" src="images/front-banner.png" alt="dropkeys" />
                </div>
            </div>
        </div>
    </header>
    <section>
        <div class="content-section-b">
            <div class="container">
                <div class="row row-teaser-icon">
                    <div class="col-md-4 main-column top">
                        <div class="teaser-icon">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/frontpage/defina.png" meta:resourcekey="Image1Resource1" />
                        </div>
                        <h2 class="text-center section-heading">Define</h2>
                        <p class="text-center lead">
                            Use our <strong class="txt-color-blueMedium">DropKeys</strong> and include them into your own library. If you can’t find what you’re looking for, you can create your own Dropkeys, those that best meet your needs..
                        </p>
                    </div>
                    <div class="col-md-4 main-column top">
                        <div class="teaser-icon">
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/images/frontpage/navega.png" meta:resourcekey="Image3Resource1" />
                        </div>
                        <h2 class="text-center section-heading">Share</h2>
                        <p class="text-center lead">
                            Share your own <strong class="txt-color-blueMedium">Dropkeys</strong> and compare your results with the rest of the community. Find and connect with other people interested in the same Data and create knowledge by sharing.
                        </p>
                    </div>
                    <div class="col-md-4 main-column top">
                        <div class="teaser-icon">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/frontpage/configura.png"
                                meta:resourcekey="Image2Resource1" />
                        </div>
                        <h2 class="text-center section-heading">Analyze</h2>
                        <p class="text-center lead">
                            Use our Social Business Intelligence engine to make dashboards. Populate them with your own graphs and formulas, then analyze your results in the time frame that best suits you.
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <div class="content-section-b no-padding">
            <div class="container">
                <div class="row">
                    <div class="col-lg-6 col-lg-offset-1 col-sm-push-5 col-sm-6">
                        <hr class="section-heading-spacer">
                        <div class="clearfix"></div>
                        <h2 class="section-heading">SEARCH, FIND or CREATE NEW DROPKEYS...</h2>
                        <p class="lead">Use our <a href="search.aspx">search engine</a> to find all kinds of data (indicators, metrics, kpis,...) or add new data into our Big Data Community Catalogue in a fast and easy way.</p>
                        <p class="lead">If you can’t find the indicator you are looking for, you can create it from scratch and then incorporate it into your private library.</p>
                        <h2 class="section-heading">...AND FILL THEM IN WITH YOUR DATA</h2>
                        <p class="lead">You can manually fill in your data sets or import them from standard files likeexcel, csv, xml or json. In just a few seconds you will have your data available to analyze.</p>
                    </div>
                    <div class="col-lg-5 col-sm-pull-7 col-sm-7">
                        <img id="imgDefault1" class="img-responsive margin-top-10 animate" src="images/frontpage/indicators.jpg" alt="">
                        <div class="text-center">
                            <a href="http://www.freepik.com/free-photos-vectors/infographic" class="infographic-link">Infographic vector designed by Freepik</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="content-section-b">
            <div class="container">
                <div class="row">
                    <div class="col-lg-6 col-sm-6">
                        <hr class="section-heading-spacer">
                        <div class="clearfix"></div>
                        <h2 class="section-heading">MAKE SENSE OF YOUR DATA WITH ATTRIBUTES</h2>
                        <p class="lead">You can define new attributes or use the existing ones to make sense of your data. Group them in the time frames that you consider most appropriate (annual, semiannual, quarterly...)</p>
                        <p class="lead">You can group multiple data sets containing same attributes but with different sampling frequency.</p>
                    </div>
                    <div class="col-lg-5 col-lg-offset-1 col-sm-6">
                        <img id="imgDefault2" class="img-responsive animate" src="images/frontpage/fill-data.jpg" alt="">
                        <div class="text-center">
                            <a href="http://www.freepik.com/free-photos-vectors/infographic" class="infographic-link">Infographic vector designed by Freepik</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="download" class="content-section-a text-center">
            <div class="download-section">
                <div class="container">
                    <div class="col-lg-8 col-lg-offset-2">
                        <h2>HELP US CREATE A COLLECTIVE INTELLIGENCE</h2>
                        <h5>It is impossible for everyone to know everything, but anyone may be an expert in something</h5>
                        <br />
                        <asp:LoginView ID="LoginViewLoggedUser" runat="server">
                            <AnonymousTemplate>
                                <div class="margin-top-10">
                                    <a href="#" class="btn btn-dropkeys login-trigger" data-toggle="modal" data-target="#myModal"><span class="glyphicon glyphicon-user"></span>&nbsp;Sign up!</a>
                                </div>
                            </AnonymousTemplate>
                        </asp:LoginView>
                    </div>
                </div>
            </div>
        </div>
        <div class="content-section-b">
            <div class="container">
                <div class="row">
                    <div class="col-lg-5 col-sm-6">
                        <hr class="section-heading-spacer" />
                        <div class="clearfix"></div>
                        <h2 class="section-heading">SHARE YOUR DATA...</h2>
                        <p class="lead">Build dashboards, add graphs, define formulas...</p>
                        <p class="lead">Make public your dropkeys and receive information from other users that help you make decisions. Use the aggregation functions (average, maximum, minimum) to develop more complex dropkeys.</p>
                    </div>
                    <div class="col-lg-5 col-lg-offset-2 col-sm-6">
                        <img id="imgDefault3" class="img-responsive animate" src="images/frontpage/dashboards.jpg" alt="">
                    </div>
                </div>
            </div>
        </div>
        <div class="content-section-b">
            <div class="container">
                <div class="row">
                    <div class="col-lg-5 col-lg-offset-1 col-sm-push-6 col-sm-6">
                        <hr class="section-heading-spacer">
                        <div class="clearfix"></div>
                        <h2 class="section-heading">...AND MAKE A COMMUNITY</h2>
                        <p class="lead">Share your dropkeys, connect with people and compare your data with them.</p>
                        <p class="lead">Export your results and publish them to other websites in just a few steps.</p>
                    </div>
                    <div class="col-lg-5 col-sm-pull-6 col-sm-6">
                        <img id="imgDefault4" class="img-responsive animate" src="images/frontpage/social-network.jpg" alt="">
                        <div class="text-center">
                            <a href="http://www.freepik.com/free-photos-vectors/infographic" class="infographic-link">Infographic vector designed by Freepik</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="ContentScript" runat="server" ContentPlaceHolderID="ScriptsContentPlaceHolder">
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/default.js") %>" type="text/javascript"></script>
</asp:Content>
