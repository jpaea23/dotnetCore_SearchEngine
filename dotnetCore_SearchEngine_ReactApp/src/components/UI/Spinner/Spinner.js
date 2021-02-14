import React from 'react';
import BeatLoader from 'react-spinners/BeatLoader';
import PropTypes from 'prop-types';

const Spinner = (props) => {
  return (
    <div>
        <BeatLoader color="aqua" css={props.override}  size={props.spinSize} />
    </div>
  );
};

export default Spinner;