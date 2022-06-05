import React from 'react';
import CampaingTable from './parserTable/campaignTable.jsx';

class Body extends React.Component{
 
    constructor(){
        super();
        this.state = {ads:null}
    } 

    async componentDidMount(){
        const groupByCampaignId = this.groupBy("campaignId");
        try {
            let adsList = await fetch('/api/ads/all');
            let result = await adsList.json();
            result.map(function(item) {
                let stopFlag = false;
                let first = new Date(item.promotionEndDate[0]);
                for (let index = 1; index < item.promotionEndDate.length; ++index) {
                    if (item == null) continue;
                    let curDate = new Date(item.promotionEndDate[index]);
                    if(first.getTime() !== curDate.getTime()){
                        item.promotionEndDate='Было распознано несколько разных дат. Проверьте указанную информацию';
                        stopFlag = true;
                        break;
                    }
                }
                if(stopFlag!=true) item.promotionEndDate=first.toDateString();
            });
            
            let campaignGroupsResult = groupByCampaignId(result);
            this.setState({ads:campaignGroupsResult})
          } catch (e) {
                let a = e;
                console.log(`Error! ${a}`);
          }
        
    }

    render() {
        let table = [];
        if (this.state.ads!=null){
            for (const [key, value] of Object.entries(this.state.ads)) {
                table.push(<CampaingTable campaignId={key} ads={value}/>)
            }
        } 
        return (
            <div className="container">
                <main role="main" className="pb-3">
                    {table}
                </main>
            </div>
        );
    }

    groupBy(key) {
        return function group(array) {
          return array.reduce((acc, obj) => {
            const property = obj[key];
            acc[property] = acc[property] || [];
            acc[property].push(obj);
            return acc;
          }, {});
        };
      }
}
  
export default Body;