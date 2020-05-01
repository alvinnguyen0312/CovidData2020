import React, { useState, useEffect } from "react";
import Tables from "./components/tables";
import CountryBox from "./components/countryBox";
import BarChart from "./components/barchart";
import LineChart from "./components/linechart";
/** @jsx jsx */
import { css, jsx } from "@emotion/core";
import "./App.css";

function App() {
  const [recordsData, setRecordsData] = useState([]);
  const [countryData, setCountryData] = useState([]);
  const [recordsDataInPeriod, setRecordsDataInPeriod] = useState([]);

  useEffect(() => {
    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const fetchData = async () => {
    let responseCountry = await fetch(
      "https://localhost:44390/api/coviddata/country",
      {
        method: "GET",
        header: { "Content-Type": "application/json" },
      }
    );

    let jsonCountry = await responseCountry.json();
    setCountryData(jsonCountry);
    getToTalRecordsInNumber();
  };
  const getDataByPeriod = async (countryId, period) => {
    let response = await fetch(
      `https://localhost:44390/api/coviddata/country/${countryId}?period=${period}`,
      {
        method: "GET",
        header: { "Content-Type": "application/json" },
      }
    );
    let json = await response.json();
    setRecordsDataInPeriod(json);
  };

  const getDataByCountry = async (countryId) => {
    let response = await fetch(
      `https://localhost:44390/api/coviddata/country/${countryId}`,
      {
        method: "GET",
        header: { "Content-Type": "application/json" },
      }
    );
    let json = await response.json();
    setRecordsData(json);
  };
  const getToTalRecordsInNumber = async () => {
    let response = await fetch(`https://localhost:44390/api/coviddata/total`, {
      method: "GET",
      header: { "Content-Type": "application/json" },
    });
    let json = await response.json();
    setRecordsData(json);
  };
  const handleCountryChange = (e, selectedOption) => {
    var selectedCountry = selectedOption ? selectedOption : "";
    if (selectedCountry) {
      getDataByCountry(selectedCountry.countryId);
      getDataByPeriod(selectedCountry.countryId, 120);
    } else {
      getToTalRecordsInNumber();
    }
  };
  console.log(recordsData);
  return (
    <div>
      <CountryBox
        countries={countryData}
        handleCountryChange={handleCountryChange}
      />
      <Tables data={recordsData} dataInPeriod={recordsDataInPeriod} />
      <BarChart data={recordsData} />
      <LineChart data={recordsDataInPeriod} />
      <div
        css={css`
          font-size: 20px;
          text-align: center;
        `}
      >
        &copy;2020 - Alvin Nguyen
      </div>
    </div>
  );
}

export default App;
