import React, { Fragment } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
} from "@material-ui/core";
import Title from "./title";
const Tables = (props) => {
  return (
    <Fragment>
      <Title>Covid Data</Title>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Date</TableCell>
            <TableCell>Confirmed</TableCell>
            <TableCell>Deaths</TableCell>
            <TableCell>Recovered</TableCell>
            <TableCell>Active</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {props.data.map((row) => (
            <TableRow key={row.recordId}>
              <TableCell>{row.recordDate.toString().split("T")[0]}</TableCell>
              <TableCell>{row.confirmed.toLocaleString()}</TableCell>
              <TableCell>{row.deaths.toLocaleString()}</TableCell>
              <TableCell>{row.recovered.toLocaleString()}</TableCell>
              <TableCell>
                {(
                  row.confirmed -
                  (row.recovered + row.deaths)
                ).toLocaleString()}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </Fragment>
  );
};

export default Tables;
