import React from "react";
import { TextField } from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";

const CountryBox = (props) => {
  return (
    <Autocomplete
      options={props.countries}
      style={{ width: 300, margin: "20px auto" }}
      autoHighlight
      getOptionLabel={(option) => option.countryName}
      renderOption={(option) => option.countryName}
      renderInput={(params) => (
        <TextField {...params} label="Choose a country" variant="outlined" />
      )}
      onChange={props.handleCountryChange}
    />
  );
};

export default CountryBox;
