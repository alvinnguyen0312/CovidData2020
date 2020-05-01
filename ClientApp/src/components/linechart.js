import React, { useRef } from "react";
import Chartjs from "chart.js";

const LineChart = (props) => {
  const chartContainer = useRef(null);
  var dates = [];
  var confirmed = [];
  var recovered = [];
  var deaths = [];
  var active = [];

  props.data.reverse().forEach((rec) => {
    dates.push(rec.recordDate.toString().split("T")[0]);
    confirmed.push(rec.confirmed);
    deaths.push(rec.deaths);
    recovered.push(rec.recovered);
    active.push(rec.confirmed - (rec.deaths + rec.recovered));
  });

  const pointRadius = 3;
  const padding = 50;
  const chartConfig = {
    type: "line",
    data: {
      labels: dates.reverse(),
      datasets: [
        {
          label: "Confirmed",
          data: confirmed.reverse(),
          borderColor: "rgba(255, 99, 132, 1)",

          fill: false,
          pointRadius: pointRadius,
        },
        {
          label: "Deaths",
          data: deaths.reverse(),
          borderColor: "rgba(54, 162, 235, 1)",
          fill: false,
          pointRadius: pointRadius,
        },
        {
          label: "Recovered",
          data: recovered.reverse(),
          borderColor: "rgba(255, 206, 86, 1)",
          fill: false,
          pointRadius: pointRadius,
        },
        {
          label: "Active",
          data: active.reverse(),
          borderColor: "rgba(153, 102, 255, 1)",
          fill: false,
          pointRadius: pointRadius,
        },
      ],
    },
    options: {
      animation: {
        duration: 1000,
        easing: "easeInOutQuint",
      },
      layout: {
        padding: {
          left: padding,
          right: padding,
          top: padding,
          bottom: 10,
        },
      },
      legend: {
        labels: {
          boxWidth: 40,
          padding: 10,
        },
      },
      title: {
        display: false,
      },
      scales: {
        xAxes: [
          {
            ticks: {
              maxTicksLimit: 10,
              maxRotation: 0,
            },
          },
        ],
      },
    },
  };

  if (chartContainer && chartContainer.current) {
    const newChartInstance = new Chartjs(chartContainer.current, chartConfig);
  }
  // this param means rerun effect if this param changes

  return (
    <div>
      <canvas ref={chartContainer} />
    </div>
  );
};

export default LineChart;
