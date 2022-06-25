import React from 'react'
import BootstrapTable from 'react-bootstrap-table-next';

const columns = [{
  dataField: 'id',
  text: 'Ad Id'
}, {
  dataField: 'adGroupId',
  text: 'Group Id'
 },
// {
//     dataField: 'type',
//     text: 'Type'
// },{
//     dataField: 'status',
//     text: 'Status'
// },
{
    dataField: 'textAd',
    text: 'Text'
},{
    dataField: 'title',
    text: 'Title'
},{
    dataField: 'promotionEndDate',
    text: 'End Date'
}];

class CampaingTable extends React.Component {
    render() {
      return (
        <div>
            <div>{'Campaign Id:' + this.props.campaignId}</div>
            <BootstrapTable keyField='id' data={ this.props.ads } columns={ columns } />
        </div>
      );
    }
  }
   
  export default CampaingTable;