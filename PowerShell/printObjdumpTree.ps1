Param(
    [parameter(mandatory=$true)]
    [String]
    $filePath
)

Set-Variable -Name KEY_REG -value ">:" -Option Constant
Set-Variable -Name CALL_FUNC -value "call[ ]+([0-9]+ [\S]+)" -Option Constant

#map<key, list<String>>
$stack  = New-Object System.Collections.Specialized.OrderedDictionary
Set-Variable -Name ARR_KEY_FUNC -value 0 -Option Constant
Set-Variable -Name ARR_KEY_HIERARCHY -value 1 -Option Constant
function addStack([System.Collections.Specialized.OrderedDictionary]$o_stack, [String]$i_stack_key, [String]$i_value_key) {
    if ($o_stack.Contains($i_stack_key)) {
        $local:valueArr = $o_stack[$i_stack_key]
    } else {
        $local:valueArr = [Collections.ArrayList]::new()
    }

    # i_value_key(callのアドレスには先頭0がないので追加)
    [void]$valueArr.add("0" + $i_value_key)
    $o_stack[$i_stack_key]=$valueArr
}

function filereadToStack([String]$i_file) {
    # objdumpファイルの読み込み→配列展開
    $key = ""
    Get-Content $i_file | Select-String -Pattern $KEY_REG,$CALL_FUNC | ForEach-Object {
        $reged = [regex]::Match($_.ToString(), $CALL_FUNC)
        if ($_.ToString().Contains($KEY_REG)) {
            #最後のセミコロン消す
            $key = $_.ToString().Substring(0, $_.ToString().Length - 1)
        } elseif($reged | Select-Object -ExpandProperty Success){
            $value = $reged.captures.groups[1].Value
            addStack $stack $key $value
        }
    }
}

function printStack([Collections.ArrayList]$list, [Int]$hierarchy) {
    $list | ForEach-Object {
        (" " * $hierarchy ) + $_
        if ( $stack.Contains($_) ) {
            printStack $stack[$_] ($hierarchy + 1)
        }
    }
}

#ここからmain

#ファイルを読み込んで$stackに展開
filereadToStack $filePath
#$stack | ForEach-Object {$_}

$stack.Keys | ForEach-Object {
    $_
    printStack $stack[$_] 1
}