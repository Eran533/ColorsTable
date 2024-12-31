<%@ Page Language="VB" AutoEventWireup="true" CodeFile="ColorTable.aspx.vb" Inherits="ColorTable" %>

<!DOCTYPE html>
<html>
<head>
    <title>Color Management</title>
    <link rel="stylesheet" type="text/css" href="../styles/styles.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
<link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
    <script src="../scripts/ColorTableScript.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Color Management</h2>
            <div id="addForm">
                <input type="text" id="colorName" placeholder="Color Name" />
                <input type="number" id="price" placeholder="Price" />
                <input type="number" id="displayOrder" placeholder="Display Order" />
                <input type="checkbox" id="inStock" /> In Stock
                <button type="button" onclick="addColor()">Add Color</button>
            </div>
            <table id="colorTable" class="table">
                <thead>
                    <tr>
                        <th>Color Name</th>
                        <th>Price</th>
                        <th>Display Order</th>
                        <th>In Stock</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
