import React from 'react';

function Layout(props){
    return(
        <div>
            <main className="p-5">
                {props.children}
            </main>
        </div>
    );
}

export default Layout;