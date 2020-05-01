import React, { useRef } from "react";
import Chartjs from "chart.js";

const BarChart = (props) => {
  const chartContainer = useRef(null);
  var recordArr = [];
  props.data.map((rec) => {
    recordArr.push(rec.confirmed);
    recordArr.push(rec.deaths);
    recordArr.push(rec.recovered);
    recordArr.push(rec.confirmed - (rec.deaths + rec.recovered));
    return recordArr;
  });

  const chartConfig = {
    type: "bar",
    data: {
      labels: ["Confirmed", "Deaths", "Recovered", "Active"],
      datasets: [
        {
          label: "# of Cases",
          data: recordArr,
          //data: [5, 14, 33],
          backgroundColor: [
            "rgba(255, 99, 132, 0.2)",
            "rgba(54, 162, 235, 0.2)",
            "rgba(255, 206, 86, 0.2)",
            "rgba(153, 102, 255, 0.2)",
          ],
          borderColor: [
            "rgba(255, 99, 132, 1)",
            "rgba(54, 162, 235, 1)",
            "rgba(255, 206, 86, 1)",
            "rgba(153, 102, 255, 1)",
          ],
          borderWidth: 1,
        },
      ],
    },
    options: {
      scales: {
        yAxes: [
          {
            ticks: {
              beginAtZero: true,
            },
          },
        ],
      },
    },
  };

  if (chartContainer && chartContainer.current) {
    const newChartInstance = new Chartjs(chartContainer.current, chartConfig);
  }

  return (
    <div
      style={{
        margin: "50px 20px 0 2px",
      }}
    >
      <canvas ref={chartContainer} />
    </div>
  );
};

export default BarChart;
