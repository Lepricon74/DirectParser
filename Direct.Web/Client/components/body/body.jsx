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
                let prev = null;
                if(item.promotionEndDate[0]!=null){
                    prev = new Date(item.promotionEndDate[0]);
                }
                for (let index = 1; index < item.promotionEndDate.length; ++index) {
                    if (item.promotionEndDate[index] == null) continue;
                    let curDate = new Date(item.promotionEndDate[index]);
                    if(prev==null){
                        prev = curDate;
                        continue;
                    }
                    if(prev.getTime() !== curDate.getTime()){
                        item.promotionEndDate='В объявлении распознано несколько разных дат окончания акций. Проверьте указанную информацию';
                        stopFlag = true;
                        break;
                    }
                }
                if(prev==null) item.promotionEndDate='В объявлении нет акций';
                else{
                    if(stopFlag!=true) item.promotionEndDate=prev.toDateString();
                }
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
        else{table.push(<img className='loader' src='./images/spinner.gif' alt="spinner"/>)}
        return (
            <div className='container'>
                <main role='main' className='pb-3'>
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

