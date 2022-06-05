import React from 'react';
  
class Header extends React.Component {
    render() {
        return (
        <header>
            <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div className="container">
                    <img className="navbar-brand" width="300" src='static/images/headerImage.png' alt="logo"/>
                    {/* <a className="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">Direct.Web.React</a>
                    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                            aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>                */}
                </div>
            </nav>
        </header>
        );
    }
}
  
export default Header;