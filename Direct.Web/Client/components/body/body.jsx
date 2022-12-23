import React from 'react';
import CampaingTable from './parserTable/campaignTable.jsx';

class Body extends React.Component {

    constructor() {
        super();
        this.state = { ads: null, filterDate: null, filterOrder: null, originalArr: null, pageType: 'text', imageTextArr: [] }
    }

    onChangeCalendar() {
        //Выключить/включить второе поле для ввода даты
        const endDateCalendar = document.getElementById('end-date');
        const startDateCalendar = document.getElementById('start-date');
        if (startDateCalendar.value === '') {
            endDateCalendar.value = ''
            endDateCalendar.disabled = true;
            return;
        }
        endDateCalendar.disabled = false;
    }

    filterAds() {
        let select = document.getElementById('filter-select');
        let inputStartDate = document.getElementById('start-date');
        let inputEndDate = document.getElementById('end-date');

        let selectValue = select.options[select.selectedIndex].value;
        let startDate = null;
        let endDate = null;
        //Показ ошибки, если второе поле не заполнено
        if (inputStartDate.value !== '' && inputEndDate.value === '') {
            alert("Укажите вторую дату для фильтрации объявлений")
            return;
        }
        if (inputStartDate.value !== '' && inputEndDate.value !== '') {
            startDate = new Date(inputStartDate.value);
            endDate = new Date(inputEndDate.value);
        }

        const filterArr = {};
        const filterInnerArr = [];
        for (let arr of Object.values(this.state.originalArr)) {
            //Фильтр по двум датам
            const filterDate = endDate === null ? arr : arr.filter(item => new Date(item.promotionEndDate) >= startDate &&
                new Date(item.promotionEndDate) <= endDate);
            if (selectValue === '0') {
                filterInnerArr.push(filterDate);
                continue;
            }
            //Получение строк, в которых нет даты
            const stringsRow = arr.filter(item => item.promotionEndDate == 'В объявлении нет акций');
            let innerArr = filterDate.filter(item => item.promotionEndDate != 'В объявлении нет акций');
            //Сортировка согласно выбранному значению в input
            innerArr.sort((a, b) => {
                return selectValue === '1' ? new Date(b.promotionEndDate) - new Date(a.promotionEndDate) :
                    new Date(a.promotionEndDate) - new Date(b.promotionEndDate);
            })
            //Если есть сортировка по датам, то не добавлять строки без даты в конце
            filterInnerArr.push(inputEndDate.value !== '' ? innerArr : innerArr.concat(stringsRow));
        }
        let index = 0;
        for (let key in this.state.originalArr) {
            filterArr[key] = filterInnerArr[index];
            index++;
        }
        this.setState({ ads: filterArr })
    }


    async componentDidMount() {
        const groupByCampaignId = this.groupBy("campaignId");
        try {
            const result = await (await fetch('/api/ads/all')).json();
            const imageText = await (await fetch('/api/adimages/all')).json();
            this.setState({ imageTextArr: imageText })
            result.map(function (item) {
                let stopFlag = false;
                let prev = null;
                if (item.promotionEndDate[0] != null) {
                    prev = new Date(item.promotionEndDate[0]);
                }
                for (let index = 1; index < item.promotionEndDate.length; ++index) {
                    if (item.promotionEndDate[index] == null) continue;
                    let curDate = new Date(item.promotionEndDate[index]);
                    if (prev == null) {
                        prev = curDate;
                        continue;
                    }
                    if (prev.getTime() !== curDate.getTime()) {
                        item.promotionEndDate = 'В объявлении распознано несколько разных дат окончания акций. Проверьте указанную информацию';
                        stopFlag = true;
                        break;
                    }
                }
                if (prev == null) item.promotionEndDate = 'В объявлении нет акций';
                else {
                    if (stopFlag != true) item.promotionEndDate = prev.toDateString();
                }
            });
            let campaignGroupsResult = groupByCampaignId(result);
            this.setState({ originalArr: campaignGroupsResult })
            this.setState({ ads: campaignGroupsResult })
        } catch (e) {
            let a = e;
            console.log(`Error! ${a}`);
        }
    }

    choosePageType(page) {
        if (page === 'text'){
            this.setState({ ads: this.state.originalArr })
        }
        this.setState({ pageType: page })
    }

    render() {
        const tableText = [];
        const tableImg = [];
        if (this.state.ads != null) {
            for (const [key, value] of Object.entries(this.state.ads)) {
                tableText.push(<CampaingTable campaignId={key} ads={value} />)
            }
        }
        else { tableText.push(<img className='loader' src='./images/spinner.gif' alt="spinner" />) }
        for (let el of this.state.imageTextArr) {
            tableImg.push(<tr><td><a target='_blank' href={el.imageUrl}>{el.imageUrl}</a></td>
                <td>{el.imageText}</td>
                <td>{el.promotionEndDate}</td>
            </tr>)
        }
        return (
            <div>
                <div className='container'>
                    <div id='changeType'>
                        <div className='form-radio'>
                            <input type="radio" id="radio-text" name='radio' defaultChecked></input>
                            <label onClick={() => this.choosePageType('text')} htmlFor="radio-text">Текст из объявлений</label>
                        </div>
                        <div className='form-radio'>
                            <input type="radio" id="radio-img" name='radio'></input>
                            <label onClick={() => this.choosePageType('image')} htmlFor="radio-img">Распознанный текст на изображениях</label>
                        </div>
                    </div>
                    {this.state.pageType === "text" ?
                        <div>
                            <div id='filter-container'>
                                <select id='filter-select'>
                                    <option value='0' selected disabled>Фильтрация объявлений по</option>
                                    <option value="1">Убыванию</option>
                                    <option value="2">Возрастанию</option>
                                    <option value="0">Без фильтра</option>
                                </select>
                                <label>От:<input className='filter-date' onChange={() => this.onChangeCalendar()} id='start-date' type='datetime-local'></input></label>
                                <label>До:<input disabled className='filter-date' id='end-date' type='datetime-local'></input></label>
                                <button id='filter-ads' onClick={() => this.filterAds()}>Отфильтровать объявления</button>
                            </div>
                            <main role='main' className='pb-3'>
                                {tableText}
                            </main></div>
                        :
                        <table id="recognizedText">
                            <thead>
                                <tr>
                                    <th>URL изображения</th>
                                    <th>Распознанный текст </th>
                                    <th>Распознанная дата </th>
                                </tr>
                            </thead>
                            <tbody>
                                {tableImg}
                            </tbody>
                        </table>}
                </div>
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

