import ReactDOM from 'react-dom';
import React from 'react';
import Header from './components/header.jsx';
import Body from './components/body/body.jsx';
import Footer from './components/footer.jsx';
  
ReactDOM.render(
    <div>
        <link rel="stylesheet" href="./static/css/site.css" />
        <Header/>
        <Body/>
        <Footer/>
    </div>,
    document.getElementById("app")
);