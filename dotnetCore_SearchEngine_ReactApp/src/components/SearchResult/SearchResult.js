import React from 'react';

function SearchResult(props){
    return(
        <div className="d-flex flex-column">
            <h4 className="p-1" >Result</h4>
            <p className="p-1">{props.Result}</p>
        </div>
    );
}

export default SearchResult;