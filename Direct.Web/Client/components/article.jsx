const React = require("react");
  
class Article extends React.Component{
 
    constructor(props){
        super(props);
        this.state = {ads:null}
    } 

    async componentDidMount(){
        try {
            let adsList = await fetch('/api/ads/all');
            let result = adsList.json();
            this.SetState({ads:result});
          } catch (e) {
            console.log(`Error! ${e}`);
          }
        
    }

    render() {
        return (
            <div>
                <div>{this.props.content}</div>
                <div>{this.state.ads}</div>
            </div>
        );
    }
}
  
module.exports = Article;