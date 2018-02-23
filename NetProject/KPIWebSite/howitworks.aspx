<%@ Page Title="" Language="C#" MasterPageFile="~/masterpages/NormalPage.master" AutoEventWireup="true" CodeFile="howitworks.aspx.cs" Inherits="howitworks" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContentPlaceHolder">
</asp:Content>
<asp:Content ID="CenterContent" runat="server" ContentPlaceHolderID="EntirePagePlaceHolder">
    <!-- Header -->
    <section class="text-center">
        <div class="howitworks-section">
            <div class="container">
                <div class="col-lg-8 col-lg-offset-2">
                    <h2>In God we trust</h2>
                    <h1>All others must bring Data</h1>
                    <hr class="star-primary">
                    <p>W. Edwards Deming.</p>
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
    </section>
    <section class="content-section-a">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <h2 class="section-heading">What is DropKeys?</h2>
                    <hr class="star-primary"/>
                    <h3 class="section-subheading text-muted">It’s a social data platform that builds knowledge through sharing data</h3>
                </div>
            </div>
            <div class="row text-center margin-top-lg">
                <div id="imgColLeft" class="col-md-4">
                    <span class="fa-stack fa-4x">
                        <i class="fa fa-circle fa-stack-2x txt-color-blueMedium"></i>
                        <i class="fa fa-group fa-stack-1x fa-inverse"></i>
                    </span>
                    <h4 class="service-heading">COLLECTIVE INTELLIGENCE</h4>
                    <p>
                        DROPKEYS is a platform based on collective intelligence that is continuously updated by the community. Every user can contribute their knowledge to this collective intelligence by adding metrics, attributes, and data.
                    </p>
                </div>
                <div id="imgColMed" class="col-md-4">
                    <span class="fa-stack fa-4x">
                        <i class="fa fa-circle fa-stack-2x txt-color-blueMedium"></i>
                        <i class="fa fa-code-fork fa-stack-1x fa-inverse"></i>
                    </span>
                    <h4 class="service-heading">BIG DATA NETWORK</h4>
                    <p>
                        You can use the data for your own personal goals or share it with everyone. Sharing your data gives you the option to compare it and receive feedback. By using complex formulas you can combine DROPKEYS previously shared by other users. These formulas allow you to reveal potential relationships between DROPKEYS, allowing you to build a Big Data Network.
                    </p>
                </div>
                <div id="imgColRight" class="col-md-4">
                    <span class="fa-stack fa-4x">
                        <i class="fa fa-circle fa-stack-2x txt-color-blueMedium"></i>
                        <i class="fa fa-bar-chart-o fa-stack-1x fa-inverse"></i>
                    </span>
                    <h4 class="service-heading">REAL-TIME INTELLIGENCE BUSINESS</h4>
                    <p>
                        DROPKEYS offer its users a free online and real time business intelligence platform. DROPKEYS integrates interactive graphics, dashboards, alerts, and messages to create a powerful and informative user experience. Graphs come alive, with the live continuously updating data of the community.
                    </p>
                </div>
            </div>
        </div>
    </section>
    <section id="about" class="content-section-b">
        <div class="container">
            <div class="row animate-title">
                <div class="col-lg-12 text-center">
                    <h2 class="section-heading">First Steps</h2>
                    <hr class="star-primary"/>
                </div>
            </div>
            <div class="row margin-top-lg">
                <div class="col-lg-12">
                    <ul class="timeline">
                        <li>
                            <div class="timeline-image">
                                <img class="img-circle img-responsive" src="images/howitworks/step1.jpg" alt=""/>
                            </div>
                            <div class="timeline-panel">
                                <div class="timeline-heading">
                                    <h4>Create</h4>
                                    <h4 class="subheading">Create your first Dropkey</h4>
                                </div>
                                <div class="timeline-body">
                                    <p>Chose a name, unit of measure and select the aggregation function that will be applied to the data in order to analize it in differents time scales.</p>
                                </div>
                            </div>
                        </li>
                        <li class="timeline-inverted">
                            <div class="timeline-image">
                                <img class="img-circle img-responsive" src="images/howitworks/step2.jpg" alt=""/>
                            </div>
                            <div class="timeline-panel">
                                <div class="timeline-heading">
                                    <h4>Define</h4>
                                    <h4 class="subheading">Define its Features</h4>
                                </div>
                                <div class="timeline-body">
                                    <p>Asign as many features as you want. As more features, more sence will have your data. Be sure to explain them as better as you can. This will give the possibility to other people, understand it in the future.</p>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="timeline-image">
                                <img class="img-circle img-responsive" src="images/howitworks/step3.jpg" alt=""/>
                            </div>
                            <div class="timeline-panel">
                                <div class="timeline-heading">
                                    <h4>Populate</h4>
                                    <h4 class="subheading">Data & Datasets</h4>
                                </div>
                                <div class="timeline-body">
                                    <p>Organize your data by adding Datasets with specific features values. Once done, you will be able to fill them with your oun data from scratch or import them from files (csv, xsl, json,...)</p>
                                </div>
                            </div>
                        </li>
                        <li class="timeline-inverted">
                            <div class="timeline-image">
                                <img class="img-circle img-responsive" src="images/howitworks/step4.jpg" alt=""/>
                            </div>
                            <div class="timeline-panel">
                                <div class="timeline-heading">
                                    <h4>Analyze</h4>
                                    <h4 class="subheading">Graphs & Dashboards</h4>
                                </div>
                                <div class="timeline-body">
                                    <p>Use your desktop page to build complex dashboards and insert the as many graph as you need. In a few steps, you will obtain your results.</p>
                                    <p>Applying feature filters, you will be able to focus on the data you wish. Furthermore, you can combine diferents dropkeys using formulas that will identify the relationship between them.</p>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </section>
    <section id="more" class="content-section-b">
        <div class="container">
            <div class="row margin-top-lg animate-title">
                <div class="col-lg-12 text-center">
                    <h2 class="section-heading">Want more?</h2>
                    <hr class="star-primary"/>
                </div>
            </div>
            <div class="row margin-top-lg margin-bottom-lg">
                <div class="col-lg-12">
                    <ul class="timeline">
                        <li>
                            <div class="timeline-image">
                                <img class="img-circle img-responsive" src="images/howitworks/step5.jpg" alt=""/>
                            </div>
                            <div class="timeline-panel">
                                <div class="timeline-heading">
                                    <h4>Share</h4>
                                    <h4 class="subheading">Make your data public</h4>
                                </div>
                                <div class="timeline-body">
                                    <p>Share as many as Dropkeys you want and wait for the community response. They can provide you with data to make comparations and discover new formulas that could help you getting better results on your analysis.</p>
                                </div>
                            </div>
                        </li>
                        <li class="timeline-inverted">
                            <div class="timeline-image">
                                <img class="img-circle img-responsive" src="images/howitworks/step6.jpg" alt=""/>
                            </div>
                            <div class="timeline-panel">
                                <div class="timeline-heading">
                                    <h4>Connect</h4>
                                    <h4 class="subheading">Establish relationships</h4>
                                </div>
                                <div class="timeline-body">
                                    <p>You can make interesting relationships through your data, identifying all those users with the same objectives.</p>
                                    <p>In the future, you will be able to create groups with people interested in the same kind of data.</p>
                                </div>
                            </div>
                        </li>
                        <li class="timeline-inverted">
                            <div class="timeline-image">
                                <a href="search.aspx">
                                    <h4>Be Part
                                    <br/>
                                        Of Our
                                    <br/>
                                        Project!</h4>
                                </a>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="ContentScript" runat="server" ContentPlaceHolderID="ScriptsContentPlaceHolder">
    <!-- Codigo de página -->
    <script src="<%=Page.ResolveUrl("~/scripts/custom/pages/howitworks.js") %>" type="text/javascript"></script>
</asp:Content>
