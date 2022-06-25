import React from 'react';
  
class Header extends React.Component {
    render() {
        return (
        <header>
            <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div className="container">
                    <img className="navbar-brand" width="250" src='./images/headerImage.png' alt="logo"/>
                </div>
            </nav>
        </header>
        );
    }
}
  
export default Header;


