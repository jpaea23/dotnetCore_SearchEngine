import React from 'react';
import Spinner from '../UI/Spinner/Spinner';
import styles from './SearchForm.module.css';
import { css } from "@emotion/core";

function SearchForm(props){
    const button = (props.loading) ?
        <button className="form-control w-50 btn btn-success" type="submit">
            <Spinner spinSize={12} />
        </button>
        :
        <button className="form-control w-50 btn btn-success" type="submit">Search</button>
        
    return (
        <div className="d-flex justify-content-center">
            <div id={styles.formWrapper} className="border w-50 p-4 rounded">
                <form onSubmit={props.SubmitSearch}>
                    <div className="form-group">
                        <label className="text-white h5 font-weight-bold">Search Engine</label>
                        <input 
                            name="searchEngine"
                            onChange={props.onValueChangeHandler}
                            type="text" 
                            className="form-control"
                            placeholder="www.google.com"
                        />
                    </div>
                    <div className="form-group">
                        <label className="text-white h5 font-weight-bold">Search Keywords</label>
                        <input 
                            name="searchKeywords"
                            onChange={props.onValueChangeHandler}
                            type="text"
                            className="form-control"
                            placeholder="e-settlements"
                        />
                    </div>
                    <div className="form-group">
                        <label className="text-white h5 font-weight-bold">Search Url</label>
                        <input 
                            name="SearchTargetUrl"
                            onChange={props.onValueChangeHandler}
                            type="text"
                            className="form-control"
                            placeholder="www.sympli.com.au/"
                        />
                    </div>
                    <div className="form-group">
                        <div className="col text-center">
                            {button}
                        </div>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default SearchForm;