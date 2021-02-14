import Axios from '../../config/Axios/Axios';
import React, { useState } from 'react';
import SearchFrom from '../../components/SearchForm/SearchForm';
import SearchResult from '../../components/SearchResult/SearchResult';

function Search(props){
    const [searchData, setSearchData] = useState({
        searchEngine: '',
        searchKeywords: '',
        SearchTargetUrl: ''
    })
    const [isLoading, setIsLoading] = useState(false);
    const [result,setResult] = useState('');

    const onChangeHandler = (event) => {
        setSearchData({
            ...searchData,
            [event.target.name]: event.target.value
        });
    }

    const onFormSubmit = (event) => {
        event.preventDefault();
        setResult('');
        setIsLoading(true);
        Axios.post('api/search',searchData)
            .then(resp => {
                console.log(resp)
                setIsLoading(false);
                setResult(resp.data['result'])
            })
            .catch(err => {
                setIsLoading(false);
            })
    }

    const searchResult = (result == '') ? null : <SearchResult Result={result}/>

    return(
        <div>
            <SearchFrom 
                onValueChangeHandler={onChangeHandler}
                SubmitSearch={onFormSubmit}
                loading={isLoading}
            /> 
            {searchResult}
        </div>
    );
}

export default Search;