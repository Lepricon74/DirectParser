import React from 'react';
import CampaingTable from './parserTable/campaignTable.jsx';

class Body extends React.Component{
 
    constructor(){
        super();
        this.state = {ads:null,filterDate:null,filterOrder:null, originalArr: null}
    }

    updateAds(e) {
        let select = document.getElementById('filter-select');
        let value = select.options[select.selectedIndex].value;
        
        const filterArr = {};
            const filterRow = [];
            for (let arr of Object.values(this.state.originalArr)){
                let test = arr.filter(item => item.promotionEndDate != 'В объявлении нет акций')
                test.sort((a,b)=>{
                    return new Date(a.promotionEndDate) - new Date(b.promotionEndDate);
                })
                filterRow.push(test);
            }
            let index = 0;
            for (let key in this.state.originalArr){
                filterArr[key] = filterRow[index];
                index++;
            }
            this.setState({ads:filterArr})
    }

    async componentDidMount(){
        const groupByCampaignId = this.groupBy("campaignId");
        try {
            let result = await (await fetch('/api/ads/all')).json();
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
            this.setState({originalArr:campaignGroupsResult})
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
                <div id='filter-container'>
                <select id='filter-select' onChange ={() => this.updateAds()}>
                    <option selected disabled>Фильтрация объявлений по</option>
                    <option value="1">Убыванию</option>
                    <option value="2">Возрастанию</option>
                    <option value="0">Без фильтра</option>
   </select>
                <button id='filter-ads' onClick={() => this.updateAds()}>Отфильтровать объявления</button>
                </div>
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

